using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipExplosionEffect : MonoBehaviour
{
    public ShipManager planet;
    private void Start()
    {
        planet = FindObjectOfType<ShipManager>();

        if (planet != null)
            planet.gameOver += Explode;

        shipMat.DOColor(shipBaseColor, "_EmissionColor", 0);
    }

    [Header("Explosion Parameters")]
    public ParticleSystem explode;
    public Material shipMat;
    public Color shipBaseColor;
    public Color shipExplosionColor;
    public float shipExplosionIntesityColor = 1;
    public ParticleSystem[] steam;
    public ParticleSystem[] stars;
    public ShieldLayer[] shields;
    public Player player;
    public Material explosionMat;
    public GameObject vCam;
    public ParticleSystem miniExplosions;
    public ParticleSystem shockwave;
    public ParticleSystem rocks;

    void Explode()
    {
        FindObjectOfType<WaveManager>().KillGame();
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => {
            foreach (ParticleSystem ps in steam)
            {
                ps.Stop();
            }
            foreach (ParticleSystem ps in stars)
            {
                //ps.Stop();
            }
            foreach (ShieldLayer shield in shields)
            {
                shield.DestroyLayer();
            }

            if (player != null)
                player.enabled = false;
            Destroy(vCam);
            miniExplosions.Play();
        });
        s.AppendInterval(0.25f);
        s.AppendCallback(() => {
            GlobalEffects.instance.Screenshake(10, 0.5f, 5);
        });
        s.Append(shipMat.DOColor(shipExplosionColor * shipExplosionIntesityColor, "_EmissionColor", 3).SetEase(Ease.InOutExpo));
        s.AppendInterval(0.25f);
        s.AppendCallback(() => {
            shockwave.Play();
        });
        s.Append(explosionMat.DOFade(0.15f, "_BaseColor", 0.1f));
        s.AppendCallback(() => {
            rocks.Play();
            miniExplosions.Stop();
            Instantiate(explode, transform.position, Quaternion.identity);
            GlobalEffects.instance.Screenshake(1, 10, 10);
            AudioManager.instance.Play("core_destroyed");
            if (player != null)
                player.enabled = true;
            Destroy(planet.gameObject);
        });
        s.Append(explosionMat.DOFade(0, "_BaseColor", 0.25f));
        s.AppendInterval(5);
        s.AppendCallback(() => {
            TransitionEffect.instance.transitionOut((Application.loadedLevel + 2) % Application.levelCount);
        });
    }
}
