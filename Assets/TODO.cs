using UnityEngine;

public class TODO {
    /*
        TODO:

        der er noget galt med spawn/goal zonerne
          der er nogle gange mure igennem de aabne omraader.
          lav offset i mapGenerator 1 hoejere

        giv banerne noget spaendende uden for mappet.
          maaske noget random terrain som er hoejere end vaeggene.
          og goer det samme for vaeggene, saa de er lidt sjovere?

        change level laver flere og flere greens/enemies.
          dvs. de bliver ikke destroyed.
          og dvs. MapGenerator#changeLevel() virker ikke.
          debug den metode og ObjectSpawner#destroyObject().

        smid de fire pool GOs ind uder map GO i editoren
          det vil goere saa objectSpawner og mapSpawner ikke skal 
          bruge public referencer til dem. de kan findes med getChild i koden.
          og laeg de fire elementer allerede i map under en ny empty GO.

        fix healthbars 
          they seem to flicker a bit. 
          Maybe it's because they're resized?
          ie. not the size of the bitmap

        can I optimize / bake lighting and shadows after generating map?

        profile to get better framerate

        rotation stutters when moving

        fix inventory

        fix portal

        make the map load 100x100 blocks at a time
          and stich them together?
          should make for much faster loading and much larger maps
          but unloading will be difficult.

        go through all scripts and make as much private as possible.

        make the debug window have booleans in the editor that hides/shows
           info and make the window scale accordingly.

        fix animations

        talents:
            include skills in the talents window, maybe rename?
            salvage: get armor and power from dead enemies
            plating: more armor
            tomb raider: 10% chance of more currency
    */
}
