using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour
{

    public PlayerInfo Info;

    private MeshRenderer _renderer;

    public TMP_InputField NameText;
    public TMP_InputField InfoText;

    public TMP_InputField EventText;

    public Animator Animator;

    public GameObject WeaponMountPoint;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        Assert.IsNotNull(_renderer,"No Renderer in children for PlayerManager");

        _renderer.material.color = Info.Visual.Color;
        NameText.text = Info.Name;
        UpdateInfo();
        UpdateEvent("");
    }

    public void OnAttackMade(Attack att)
    {
        if (att.Source == Info)
        {
            Animator.Play("Attack");
            WeaponMountPoint.GetComponent<AudioSource>().clip = att.AttackMade.Visual.Audio;
            WeaponMountPoint.GetComponent<AudioSource>().Play();
            var part = Instantiate(att.AttackMade.Visual.Particles, WeaponMountPoint.transform);
            
            Destroy(part.gameObject, att.AttackMade.Visual.Particles.main.duration);
        }
    }

    public void OnAttackResult(AttackResult result)
    {
        if (result.Attack.Source == Info)
        {
            UpdateInfo();
            var resSt = result.IsHit ? "HIT" : "MISS";
            UpdateEvent($"Attack {result.Attack.AttackMade.Name} Made {resSt}");
        }
        else
        {
            if (result.IsHit)
                Animator.Play("Hit");
            else
            {
                Animator.Play("Evade");
            }

            UpdateInfo();
            var resSt = result.IsHit ? "HIT" : "MISS";
            UpdateEvent($"Attack Received {resSt} : Dam {result.Damage}");
        }
    }


    private void UpdateInfo()
    {
        var st = "HP: "+Info.HP+"\nEnergy: "+Info.Energy;
        InfoText.text = st;
    }

    private void UpdateEvent(string text)
    {
        EventText.text =text;

    }
    
}
