using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Game_Server;

namespace Redux.Managers
{
    public static class PetManager
    {
        public static ConcurrentDictionary<uint, Pet> ActivePets = new ConcurrentDictionary<uint, Pet>();

        public static void PetManager_Tick()
        {
            foreach (var pet in ActivePets.Values)
            {

                if (pet.CombatEngine != null && pet.CombatEngine.nextTrigger != 0)
                    if (Common.Clock > pet.CombatEngine.nextTrigger)
                        pet.CombatEngine.OnTimer();

                if (!pet.Alive && Common.Clock - pet.DiedAt > Common.MS_PER_SECOND * 3)
                {
                    pet.Faded();
                    if (ActivePets.ContainsKey(pet.UID))
                    {

                        Pet p;
                        ActivePets.TryRemove(pet.UID, out p);
                    }
                }
                else if (pet.Remove != true && pet.Alive)
                {
                    pet.Pet_Timer();
                }

                else if (pet.Remove == true)
                {

                    pet.PetOwner.Pet = null;
                    pet.Map.Remove(pet);
                    Pet p;
                    ActivePets.TryRemove(pet.UID, out p);
                }
            }



        }
    }
}
