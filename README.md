# SlideTableMover

# Készítsen C#-ban egy olyan WPF alkalmazást, amely képes vezérelni egy tárgylemez asztalt.
Megvalósítva
# A tárgylemez asztalt 3 léptetőmotor mozgatja az X, Y és Z tengelyek mentén (hasonlóan egyes CNC gépekhez, vagy 3D nyomtatókhoz)
Megvalósítva
# Az alkalmazás felületén legyen egy aktív felület mely mutatja a tárgyasztal aktuális pozícióját és a mozgási tartományt.
A UI 3 területre lett osztva:
- Header: Az aktuális koordináták + az aktuális motorpozíciók(0-től megtett step-ek száma)
- A vizualizációs felület
- Controlpanel
# A vizualizáció két dimenziós felületen történjen, jól látható legyen rajta a tengelyek mentén az asztal elmozdulása és pozíciója.
A két dimenziós megjelentítés miatt a Z koordinátán történő mozgás implementációja eltér az X és az Y-tól. Elsősorban az X,Y koordináták megvalósítására fókuszáltam, de a program tartalmazza a Z koordinátán való mozgás vizualizációját is a tárgylemez asztal méretének változásával.
# A vizualizációs felületen egér kattintással lehessen vezérelni az asztal pozícióját. (ha a terület bármely részére klikkelünk, akkor oda mozog)
Az kétdimenziós megjelenítés miatt ez az X és Y koordináták esetében lett megvalósítva.
# Az alkalmazás szimulálja a motorok mozgását (ne 0 idő alatt lépjen egyik pozícióból a másikba).
Az alkalmazás tartalmazza a pozícióváltás időtartamát, ez az aktív felületről szabályozható is.
# A motorok pozíciói különüljenek el az asztal, felületen is megjelenített pozíciójától, jelölje ki megfelelő referencia koordináta rendszereket, és közöttük végezzen transzformációt.
Az alkalmazás tartalmazza a koordináták és a motorpozíciók megjelenítését is. Egy koordináta lépés mindig egyenlő egy pixellel. Habár a motorstep szögfordulást jelent, jelen esetben azt tudjuk szabályozni, hogy egy step egy pixel mekkora tört részét vagy többszörösét tegye ki.
# Lehessen a tárgyasztalt külön, motoronként is mozgatni.
A controlpanel tartalmazza a motorok külön-külön indítógombjait.
# A motorok sebességét a felületről lehessen szabályozni.
A controlpanel tartalmazza a szabályozáshoz szükséges felületet.
# Használjon a tárgylemezasztalra és a motorokra külön osztályt, valamint motorstepben határozza meg a motorok aktuális pozícióit.
Az alkalmazás külön osztályokat tartalmaz a motorokra és a tárgylemezasztalra.
# A felület és a motorvezérlés egymástól jól elkülöníthető legyen (akár külön projekt is lehetséges).
A felület és a motorvezérlés elkülönül, de egy projekten belül.
# A tényleges, fizikai tárgylemez asztalt szimulált hardverrel helyettesítse az alkalmazáson belül
Megvalósítva
# Úgy tervezze meg az osztályokat, hogy gondoljon a továbbfejlesztési lehetőségekre is.
Mivel az alkalmazás igen rövid idő alatt készült el, így elsősorban a funkcionalitásra fókuszáltam.
Habár a motorok, a tárgylemez asztal és a mozgató szervíz külön osztályokat kaptak, az alkalmazás jelen állapotában kevésbé felel meg a továbbfejlesztési elvárásoknak.
Elsősorban a dependency inversion az az elvárás ami nem lett megvalósítva.
Ahhoz, hogy ennek megfeleljen:
- IMotor, ISlideTable, IMovementService interfészek létrehozására lenne szükség.
továbbá:
- A felület input mezőit és egyéb elemeit külön konponensekbe (pl. UserControl) kell szervezni.
