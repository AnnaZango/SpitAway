## INTODUCTION

SPIT AWAY is a short 2D game about alpacas spitting each other. The player takes the role of the brown alpaca and plays against the white one, who spits back using a simple AI. There are three types of saliva: normal, fire, and venom; each of them has a different impact range, weight, and damange inflicted. The player and enemy both start off with normal saliva, but potion pickups will randomly appear. 

The turns alternate between the player and the enemy, with a time of 20 seconds to act, and the scenario tiles are destructible. Follow this link to play online: 
https://sharemygame.com/@AnnaZ/spit-away

![Screenshot](/Images/Screenshot_pc1.jpg)

## DEVELOPMENT

This game has been developed with Unity version 2021.3.10f1 and Visual Studio 2022. It has been designed to be played on a computer or on Android. To play from a computer, a keyboard and a mouse are needed. 

To play from the Unity Editor, download the project and go to the MainMenu scene to start playing (Scenes/MainMenu). Alternatively, you can make a local build or a build for Android. Below you can see short videos of gameplay for both pc and Android.

### PC version:
- https://www.youtube.com/watch?v=IIFA6dFXah4&ab_channel=AnnaZango

![Screenshot](/Images/Screenshot_pc4.jpg)

### Android version:
- https://www.youtube.com/watch?v=r0ev2ezrrvc&ab_channel=AnnaZango 

![Screenshot](/Images/Screenshot_Android.jpeg)

## THIRD PARTY ASSETS
### Alpaca sprites and animations, potions, hearts and buttons:
- These assets have been developed by me, but the icons were obtained from: https://thenounproject.com/icons/

### Background Sprites:
- Craftpix: https://craftpix.net/freebies/free-cartoon-parallax-2d-backgrounds/

### Tiles:
- Game Art 2D: https://www.gameart2d.com/free-desert-platformer-tileset.html
- Game Art 2D: https://www.gameart2d.com/winter-platformer-game-tileset.html

### Particle effects:
- Jean Moreno: https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565

### Música i sfx:
- Juhani Junkala: https://juhanijunkala.com/ 
- 3xBlast, Harm-Jan Wiechers: https://opengameart.org/content/bleeps-and-synths
- Bart Kelsey: https://opengameart.org/content/8-bit-platformer-sfx
- Rubberduck: https://opengameart.org/content/80-cc0-creature-sfx 

### Font:
- Letterna studios, Balloon Dreams: https://letterena.com/product/ballon-dreams/

### Asset cleaner:
- Employed to delete unused assets from the project: 
https://github.com/unity-cn/Tool-UnityAssetCleaner


## COM S'HA DESENVOLUPAT

El joc conté diversos scripts, separats en carpetes segons el tipus. Els scripts principals son els de CameraMovement.cs, els de l'enemic i els del player. 

CameraMovement.cs:
Controla cap a on es mou la càmara i per tant què veu el jugador. Quan el torn del player/enemic està actiu, la càmara el segueix a aquest. En el moment que s'instancia un projectil, la càmera el segueix (`CameraFollowProjectile()`), a fi que el jugador sàpiga on va. Quan aquest colisiona amb alguna cosa, la càmara va cap a l'alpaca que tingui el següent torn. A fi de passar d'un torn a l'altre, quan la càmara arriba a l'alpaca que tindrà el següent torn, es crida el mètode `ChangeTurn()` de TurnController.cs i s'assigna el nou torn a qui correspongui. També hi ha mètodes públics per assignar nous targets per la càmera (ja siguin jugador/enemic o projectil).

Player scripts:

PlayerAiming.cs: controla l'aiming del jugador. Tant l'angle com la potència es controlen a través de sliders de l'escenari, i per tant s'afegeixen els mètodes corresponents:
`slider.onValueChanged.AddListener(delegate { Method(); });` 
L'angle es controla a través del mètode `Rotate()`, que rota el cap de l'alpaca, i la potència a través del mètode `ChangePower()`, que canvia la força. També controla si el jugador inicia l'escupinada a través de `StartShooting()`, que alhora indica que el jugador ha acabat el seu torn i per tant ja no pot actuar. Disparar el projectil en si s'efectúa amb el mètode `ShootProjectile()`, que és un mètode que es crida a través d'un event a l'animació de disparar de l'alpaca (Amb el script ShootEvent.cs) i que informa al script ShootingPlayer.cs que ha d'instanciar el projectil i amb quina potència. Aquest script també escolta si el temporitzador ha acabat, per efectuar el mètode `TimeTurnIsUp()`, que finalitza el torn. Quan el torn acaba, l'angle del jugador es reseteja a 0 (a fi que el joc no sigui tan fàcil i no es deixi el cap de l'alpaca tort) amb `ResetAngleAndSetIdle()`. També hi ha un mètode públic per donar o no el torn al jugador i permetre accions (`SetTurn(bool isTurn)`). 

ShootingPlayer.cs: controla tot allò que té a veure amb efectuar el tret i els projectils: amb el mètode `FireProjectile()` s'instancia el projectil i s'inicialitza amb una potència concreta (accedint al component Projectile i cridant al mètode `Initialize()`) i assignant el projectil com a objectiu de la càmara, perque el segueixi; El mètode `AssignProjectile()`permet assignat quin projectil es fa servir en aquell moment; el mètode `EnableProjectileButton()` permet activar un nou tipus de saliva.

PlayerController.cs: serveix per controlar el moviment del jugador, només efectuable si té el torn actiu. També controla les animacions en relació al moviment. Per permetre o no el salt al jugador, es fa servir el mètode extensió de rigidbody `RaycastFirstHit()`, que fa un circle cast cap avall per detectar si hi ha terra.

PlayerStats.cs: controla la vida del jugador i aquells mètodes per canviar la vida i ferir al jugador (`ChangeHealthPoints()`, `SetHealthWithinRange()`, `Hurt()`), per modificar la UI que mostra els cors (`UpdateHearts()`) i un mètode públic per accedir a la vida actual des d'altres scripts (`GetCurrentHealth()`).

Enemy scripts:

EnemyAI.cs: és el principal script de l'enemic, controla la inteligència artificial d'aquest. L'enemic farà diverses coses durant el seu torn i al final dispararà. Té una llista de localitzacions (listTargetPositions), que s'omple en funció de les condicions actuals de l'enemic, i anirà d'una a l'altra. Quan comença el torn de l'enemic (amb el mètode `SetTurn(bool giveTurn)`), s'inicia la corrutina de l'enemic amb `StartCoroutine(EnemySequence());`. Dins d'aquesta corrutina, El primer que fa és determinar una localització random des d'on dispararà (cridant el mètode `SetLocation()` de ShootingLocation.cs). Després mirarà si hi ha pickups dins del seu rang de moviment (`CheckIfPickupsInRange()`) i assignarà les prioritats de moviment amb `SetEnemyMovementPreferences()`, que depenen de si hi ha projectils aprop i de la vida que té l'enemic. Finalment s'inicia el temporitzador. Amb aquesta informació, l'enemic s'anirà movent d'una target location a una altra; quan arriba a una target location, s'assigna la següent amb `CheckIfCurrentTargetReached()`. Al final del seu torn, dispararà amb la coroutine `Shoot()`, que crida al mètode corresponent de ShootingEnemy.cs, que és `FireProjectile(CalculateVelocityToHit())`. Amb el mètode `CalculateVelocityToHit()` l'enemic calcula a quina velocitat ha de llençar el projectil per donar al player. A fi que no sempre encerti, s'aplica un offset a la punteria (`SetAccuracyOffset()`), que depèn de la vida que té l'enemic: a menys vida, més punteria té (menor és l'offset). També hi ha aquells mètodes relacionats amb la vida de l'enemic, ferir-lo, i la UI dels seus cors, així com un mètode públic per accedir a la seva vida actual.

Aquí podeu veure l'enemic anant a recollir un pickup tipus projectile (poció per disparar verí):
![Screenshot](/Images/Screenshot_pc3.jpg)

ShootingEnemy.cs: és un script molt similar al del player, que controla les mecàniques de disparar i els projectils disponibles. El tipus de saliva amb la que dispara s'assigna de manera randomitzada entre aquelles disponibles.

ShootingLocation.cs: conté el mètode `SetALocation()`, que es crida a l'inici del torn de l'enemic i determina des d'on dispararà, dins d'un rang.

I aquí podeu veure l'enemic disparant un projectil de tipus normal:
![Screenshot](/Images/Screenshot_pc2.jpg)

Controllers scripts:

TurnController.cs: controla el torn de joc amb `ChangeTurn()`. A l'inici de cada torn, crida als mètodes `InstantiateHeartPickup()` i `InstantiateProjectilePickup()` de PickupInstantiator.cs perque instanciin, si correspòn, els pickups de la zona.
SceneController.cs: conté els mètodes que permeten passar entre escenes i tancar el joc.
EndGameController.cs: serveix per mostrar els panells de final de partida, ja que conté un mètode públic que es diu `ShowPanelEnd()` i que es crida quan el player o l'enemic mor. Depenent de si el jugador guanya o perd, es mostra un panell o un altre.

Projectile scripts:
Projectile.cs i TileDestroyer.cs: serveixen pels projectils tant del player com de l'enemic. Cada projectil té un box collider tipus trigger i dos colliders circulars per determinar el rang d'impacte del projectil. Quan el projectil entra en contacte amb alguna cosa, amb `OnTriggerEnter2D()` des de Projectile.cs s'activen els colliders amb `EnableAllColliders()`. Aleshores, des del script TileDestroyer.cs (també al prefab del projectil), amb `OnCollisionEnter2D()` es detecten tots els punts de colisió i s'eliminen les tiles de l'escenari que cauen dins del rang d'impacte del projectil. 
ProjectileProperties.cs: determina les propietats de cada projectil: rang d'impacte, vida que treu, prefab de les partícules a instanciar quan hi ha impacte... i passa aquesta informació a Projectile.cs i TileDestroyer.cs.

Pickups scripts: 
PickupInstantiator.cs: serveix per instanciar els pickups de l'enemic i el player, en una localització random però dins d'una àrea determinada. La probabilitat d'instanciar un pickup de tipus projectil (`InstantiateProjectilePickup()`) és d' 1/3, mentre que la probabilitat de pickup heart (`InstantiateHeartPickup()`) depèn de la vida actual de l'enemic/player: a menor vida, més necessitat té d'un cor (es mira a través de `GetHeartNeed()`), i més probabilitat de que s'instancii. 
També trobem aquí els scripts dels pickups que s'instancien, HeartPickup.cs i ProjectilePickup.cs, que instancien cors i projectils respectivament. 

Altres scripts:

El script Extensions.cs és una extensió de Rigidbody2D i conté diversos mètodes destinats a determinar el terreny colindant des d'un rigidbody. `RaycastFirstHit()` fa un circlecast i serveix per retornar si hi ha hagut una colisió amb aquest cercle, donada una direcció, distancia, radi i layer. Es fa servir per determinar si el player o l'enemic estan tocant el terra, per tenir una major àrea de detecció que amb només un raig. `RaycastRayFirst()` fa un raig raycast, i també retorna si hi ha hagut una col·lisió d'aquest raig amb alguna cosa, donada una direcció, distancia, i layer. Es fa servir per l'enemic per determinar, des de diferents angles, si té aprop una paret o no i l'ha de saltar. `RaycastAll()` fa un circlecast però no només retorna la primera col·lisió, sinó totes. Serveix per dues funcions: 1) per l'enemic, per detectar tots aquells pickups dins del seu radi de detecció; 2) pels projectils, per detectar si ha tocat al player o a l'enemic.

GameManager.cs: controla l'estat general de la partida: si ha acabat (`Set/GetIfGameFinished()`), qui guanya (`Set/GetPlayerWins()`).

Timer.cs: controla el temporitzador pel player.
