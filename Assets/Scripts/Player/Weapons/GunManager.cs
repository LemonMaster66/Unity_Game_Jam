using System.Linq;
using PalexUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class GunManager : MonoBehaviour
{
    public int ActiveGun = 0;

    public SerializedDictionary<Gun, bool> Guns;


    private AudioManager audioManager;

    // void Awake()
    // {
    //     audioManager = GetComponent<AudioManager>();

    //     if(HasGun(ActiveGun) == true) SwapGun(ActiveGun);
    //     else GetGun(ActiveGun).gameObject.SetActive(false);

    //     foreach(Transform transform in Tools.GetChildren())
    //     {
    //         Debug.Log(transform.name);
    //         Guns.Add(transform.GetComponent<Gun>(), false);
    //     }
    // }

    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.F1)) GunState(0, true);
    //     if(Input.GetKeyDown(KeyCode.F2)) GunState(1, true);
    //     if(Input.GetKeyDown(KeyCode.F3)) GunState(2, true);
    //     if(Input.GetKeyDown(KeyCode.F4)) GunState(3, true);
    // }
    

    // public void OnFire(InputAction.CallbackContext context)
    // {
    //     if(CountGuns() < 1) return;
    //     if(context.started)       GetGun(ActiveGun).ShootStart();
    //     else if(context.canceled) GetGun(ActiveGun).ShootEnd();
    // }

    // public void OnAltFire(InputAction.CallbackContext context)
    // {
    //     if(CountGuns() < 1) return;
    //     if(context.started)       GetGun(ActiveGun).AltShootStart();
    //     else if(context.canceled) GetGun(ActiveGun).AltShootEnd();
    // }

    // public void OnScroll(InputAction.CallbackContext context)
    // {
    //     if(CountGuns() < 1) return;
    //     float inputScroll = context.ReadValue<float>();
    //     if(inputScroll != 0)
    //     {
    //         while(true)
    //         {
    //             ActiveGun -= (int)inputScroll;

    //             if(ActiveGun < 0)             ActiveGun = Guns.Count-1;
    //             if(ActiveGun > Guns.Count-1) ActiveGun = 0;

    //             if(HasGun(ActiveGun)) break;
    //         }

    //         SwapGun(ActiveGun);
    //     }
    // }

    // public void SwapGun(int GunChoice)
    // {
    //     ActiveGun = GunChoice;
    //     if(Guns.ElementAt(ActiveGun).Key.gameObject.activeSelf) return;

    //     for(int i = 0; i < Guns.Count; i++)
    //     {
    //         GetGun(i).gameObject.SetActive(false);
    //         if(i == GunChoice) GetGun(i).gameObject.SetActive(true);
    //     }

    //     //playerSFX.PlaySound(playerSFX.WeaponSwap, 1, 0.25f, 0.1f);
    // }

    // public void GunState(int SelectedGun, bool State)
    // {
    //     Gun gun = Guns.ElementAt(SelectedGun).Key;
    //     Guns[gun] = State;
    //     if(State) SwapGun(SelectedGun);
    // }

    // public int CountGuns()
    // {
    //     int result = 0;
    //     foreach(bool gunCheck in Guns.Keys) if(gunCheck == true) result++;
    //     return result;
    // }

    // public bool HasGun(int Gun)
    // {
    //     if(Guns.ElementAt(Gun).Value) return true;
    //     else return false;
    // }
    
    // public Gun GetGun(int Index)
    // {
    //     return Guns.ElementAt(Index).Key;
    // }
}