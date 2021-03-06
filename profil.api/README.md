﻿# .Net core Web Api Projektanteckningar/Steps
## Projektkrav 
(Stage1: Web Api, Registrering och inloggning)
Applikationen ska visa en lista av konsultprofiler (Klienten, vi avvaktar denna implementation ett tag)
- En profil består av Förnamn, Efternamn och en Beskrivning

Som en anonym användare kan jag
- Registrera mig
- Se förnamn på profilerna
	
Som en registrerad användare kan jag
- Se all information i konsultprofilerna

### Nya krav (Stage2: administration)
Som en innehållsadministratör kan jag
- lista alla profiler
- redigera, lägga till och ta bort profiler

## Teknikval
Eftersom projektet har som syfte att driva en bloggserie om [.Net core](https://docs.microsoft.com/sv-se/dotnet/) och [ASP.Net core](https://docs.microsoft.com/en-us/aspnet/core/) faller det sig naturligt att använda följande tekniker, samtliga servade i det stora fluffiga molnet. Och även om andra molntjänster skulle kunna användas kommer vi slutligen drifta allt i Azure.
- **API**: alla moderna system exponerar data via apier, så är det. **.Net core Web API**
- **Klient**: Kan komma att byggas i flera olika tekniker, men till att börja med blir det en mobilapp i hybridramverket **[Ionic](https://ionicframework.com/)**, som bygger på [Angular](https://angularjs.org/) och Html5. 
- **Administration** av profildata: Då vi behöver blanda in [ASP.Net core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc) (föutom apidelen) någonstans får det bli här.
- **Autentisering**: med .net core känns det självklart att anväda [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) för hantering av användare. Här skulle vi kunna nöja oss med bara Identity, men eftersom jag är en förespråkare för [IdentityServer](https://identityserver4.readthedocs.io/) gör vi implementationen med IdentityServer4 (IdS4) ovanpå core Identity. 
  - Den stora fördelen IdS4 ger oss är att vi kan använda samma autentiseringslösning för olika system, apier i olika applikationer, något som inte är ett krav här men jag väljer då jag själv inte har testat IdS4 ännu.

## Skapa nytt projekt för API
- Lägg till nytt projekt, ASP.NET Core Web Application (.NET Core)
ange namn, profil.api, välj Empty template.

- Starta appen i Debug för att verifera att du får upp en browser med meddelandet "Hello World!"

I [Program.cs](Program.cs) finns inställningarna för webbservern, t ex om vi vill köra IIS Express eller något annat, vi låter dessa vara som de är.

I [Startup.cs](Startup.cs)  konfigurerar man "service dependencies" (i metoden ConfigureServices) och  HTTP Request pipeline (i metoden Configure)

#### Http Request pipeline
*i Startup.cs > Configure*
- Http Request pipeline består av ett antal "middlewares" vilka kopplas på IApplicationBuilder (app)
vanliga  middelwares hämtar du från nuget, och du kan så klart skriva egna.
- Ordningen för middlewares är viktig eftersom en middleware tar emot data från föregående middleware, 
gör sin grej och skickar sedan vidare data till nästa i "pipen" (kedjan) [mw 1, mw 2, mw 3] och lyssnar (eventuellt) på resultatet från den/de senare i kedjan [mw 3, mw 2, mw 1]

- Vissa middelwares är "terminal" vilket betyder att de inte skickar data vidare, de är sist i kedjan; 
	några exempel är UseWelcomePage() och Run(), alla middlewares configurerade senare i kedjan kommer alltså inte att köras.

#### Öppna [Startup.cs](Startup.cs)
- och lägg till ett repository för att hämta konsultprofilerna
- med repositoryt [ConsultantProfileRepository.cs](Models/ConsultantProfileRepository.cs) på plats kan vi lägga till en controller för att låta vårt api hämta data.
- för att controllern ska hittas behövs routing, det kan man sätta direkt på controllern som vi visar i koden, alternativt i Startup.cs > Configure (eller både och, controllerns konfig väger tyngst)
exempel: 
``` language-aspnet
private void ConfigureRoutes(IRouteBuilder routeBuilder)
{
	routeBuilder.MapRoute("Default",
		"{controller=Home}/{action=Index}/{id?}"); // controller=Home >> om controller name inte hittas (eller inte angavs), redirect till HomeController med default action(metod) Index
}
// används i Configure enligt, app.UseMvc(ConfigureRoutes)
```
- nu har vi ett fungerande api som kan leverera konsultprofiler, nästa steg är att lägga till autentisering vilket vi gör med .Net Core Identity och [Identity Server 4](https://github.com/IdentityServer/IdentityServer4)

## Nytt projekt för IdentityServer och .Net Core Identity
Här följer jag exemplet från [Identity Server Quickstarts > Using ASP.NET Core Identity](https://identityserver4.readthedocs.io/en/release/quickstarts/6_aspnet_identity.html), (det finns även ett [exempelprojekt att ladda ner här](https://github.com/IdentityServer/IdentityServer4.Samples/tree/dev/Quickstarts/6_AspNetIdentity),
med [deras färdiga UI](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI)
### följ stegen från "New Project for ASP.NET Identity" till och med "Creating a user"
#### "Add IdentityServer packages" 
  - när jag uppdaterade alla paket till det senaste misslyckades nuget med restore, vilket löstes genom att uppdatera nuget till senaste beta https://docs.nuget.org/ndocs/guides/install-nuget ,  
  - efter det följde jag denna länk för uppdatering av core till .net core 1.1 https://blogs.msdn.microsoft.com/webdev/2016/11/16/announcing-asp-net-core-1-1/
#### "Configure IdentityServer" 
  - konfiguration av IdentityServer "InMemory" (dvs statisk hårdkodad konfiguration)
  - Med Identity Server på plats kan vi lägga till till Authorization på vårt Api, ConsultantProfileController.Get() > 
    - Än så länge är det öppet att hämta data från http://localhost:57624/api/consultantprofile men nu är det dags att begränsa åtkomsten.
    - Lägg till "Authorize" på Get(), om du försöker hämta data nu får du tillbaks status code "401 Unauthorized"
  - Vi behöver även konfigurera vårt api till att använda vår IdentityServer för autentisering, dvs så att vårt api godkänner tokens utfärdade av vår IdentityServer, detta gör vi i profil.api.Startup
    - Lägg till "IdentityServer4.AccessTokenValidation": "1.0.1" som en dependency i project.json
    - lägg till denna middleware i Startup.Config
#### "Creating the user database" 
  - Eftersom vi använder Identity för användarhanteringen behöver vi initiera databasen, har du som jag uppdaterat .net core till 1.1 följ dessa steg (annars får du varningen "No executable found matching command "dotnet-ef" när du försöker initiera databasen)
    - lägg til följande nugetpaket (https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)   
      - under dependencies: "Microsoft.EntityFrameworkCore.Design": {"type": "build", "version": "1.1.0"}
      - under tools: "Microsoft.EntityFrameworkCore.Tools": "1.1.0-preview4-final",
    "Microsoft.EntityFrameworkCore.Tools.DotNet": "1.1.0-preview4-final",
  - öppna en kommandoprompt i projektkatalogen för vår IdentityServer och kör följande kommandon
      - "dotnet ef migrations add InitialDbMigration" för att skapa vår baseline, detta är inte nödvändigt.
      - "dotnet ef database update" för att initiera databasen.
#### "Creating a user"
  - Starta api och IdentityServer och öppna identityserver (http://localhost:5000) 
  - Registrera en användare (komplexa lösenord, t ex Abc123!, krävs som standard)
  - Nu när vi har en användare kan vi testa vårt api från ett verktyg som [Postman](https://www.getpostman.com/)
    - anropa först IdentityServer, http://localhost:5000/connect/token, från postman med följande inställningar 
      - (client_id, client_secret, grant_type och scope kommer från Config.GetClients(), username och password är från användaren du nyss registrerade) ![Connect Token Settings](ReadMe_images/connect_token_settings.png) som svar ska du få ett json-objekt med bla a en "access_token", kopiera access_token och öppna en ny flik för att skapa ett anrop mot vårt api
    - anropa därefter vårt api enligt bilden, http://localhost:57624/api/consultantprofile, med headern "Authorization" "Bearer [tokenstring]" ![Api Anrop Bearer Token](ReadMe_images/api-anrop_bearer-token.png) och som svar bör du nu få data från apiet.

## Reflektion
Ok, nu har vi alltså satt upp ett api som vi kan anropa för att hämta konsultprofiler innehållande Förnamn, Efternamn och Beskrivning.
Men just nu måste jag vara en registrerad användare för att kunna se profildata, enligt kravet ska jag kunna se profilernas förnamn även om jag inte är registrerad.
- Här kan vi välja olika lösningar
  - Den ena är olika api actions (controller-metoder) där en action levererar data till icke autentiserade användare och en annan action levererar data till autentiserade användare, 
  - En annan lösning är att ta bort attributet "Authorize" från vår "Get"-action och kontrollera behörighet i denna.
  - Eftersom jag anser att den som frågar efter data ska veta vad den frågar efter samt att jag vill ha renare metoder i mitt api väljer jag det första alternativet, med separata actions. Visst hade det varigt trevligt om klienten bara behövde anropa en metod och sen kunde visa upp data oavsett om det var fulla profilerna eller bara förnamnen, men det är mycket tydligare för klienten att returnera "401 Unauthorized" än att returnera begränsad data, dessutom blir som sagt api-koden renare; ren kod == bra kod.
### Uppdatera api
Så i [ConsultantProfileController](Controllers/ConsultantProfileController.cs) uppdaterar vi våra actions enligt följande
```language-aspnet
[HttpGet]
[Route("")]//default Getmetod, bara för att jag vill ge metoden ett tydligare namn än "Get", 
public IEnumerable<ConsultantProfileLimitedViewModel> GetLimitedConsultantProfiles()
{
    return consultantProfileRepository.GetAllLimited();
}

[HttpGet]
[Authorize]
[Route("GetFull")]
public IEnumerable<ConsultantProfile> GetFullConsultantProfiles()
{
    return consultantProfileRepository.GetAll();
}
```
- Perfekt, 
  - http://localhost:57624/api/consultantprofile/getfull returnerar hela profilen och kräver autentisering.
  - http://localhost:57624/api/consultantprofile kräver ingen autentisering, men.. oops, vi får både för och efternamn; det stämmer inte helt med kraven.. att det ska vara så svårt att hålla reda på så få krav, det är dags att införa tester innan det blir för krångligt och innan kraven ändras, för det vet man ju att de alltid gör :)
## Nytt projekt för att testa vårt api
Jag är ganska bekväm med NUnuit, men eftersom deras [testrunner för .net core fortfarande är i alphastadiet](https://github.com/nunit/dotnet-test-nunit) känns det som att det äntligen är dags att jag sätter mig in i [xUnit ](https://xunit.github.io/docs/getting-started-dotnet-core.html) som för visso är i beta, men jag har fått intrycket av att fler använder xUnit än NUnit mot .net core; jag använder mig av [Resharpers ](https://www.jetbrains.com/help/resharper/sdk/Features/UnitTest/DotNetCore.html) testrunner (som vanligt)

- Skapa ett nytt projekt, "Profil.Api.Test", av typen Class library (.net core)
- lägg till nuget packages och config för xUnit
```language-aspnet
{

    "testRunner": "xunit",
    "dependencies": {
        "xunit": "2.2.0-beta2-build3300",
        "dotnet-test-xunit": "2.2.0-preview2-build1029"
    },
    "frameworks": {
        "netcoreapp1.1": {
            "dependencies": {
                "Microsoft.NETCore.App": {
                    "type": "platform",
                    "version": "1.1.0"
                }
            }
        }
    }
}
```
- följ instruktionerna i [xUnit getting started](https://xunit.github.io/docs/getting-started-dotnet-core.html) för att göra ett första test, nu ska du kunna köra testerna från en kommandoprompt med kommandot "dotnet test"

### Skapa tester för vårt api enligt kraven
- Lägg till en ny testklass, ConsultantProfileControllerTest, och bygg upp tester enligt kraven
  - Jag väljer här att endast testa på controllernivå eftersom det är där gränsen går mot användarna.
  - När jag försökte köra testet mot apiet fick jag följande fel
	``` 
    Unhandled Exception: System.IO.FileNotFoundEx ception: Could not load file or assembly 'Microsoft.DotNet.InternalAbstractions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'. The system cannot find the file specified.
	at Xunit.Runner.DotNet.Program.GetAvailableRunnerReporters()
    at Xunit.Runner.DotNet.Program.Run(String[] args)
    at Xunit.Runner.DotNet.Program.Main(String[] args)
	```
	lösningen var att lägga till "Microsoft.DotNet.InternalAbstractions": "1.0.0" i project.json.
- Jag stötte även på problem med mockningen av repositoriet med Moq CallBase vilket inte fungerade som tänkt, mer om det finns att läsa i ConsultantProfileControllerTest.cs 
- Efter implementationen av testerna (se i koden) uppfyller apiet kraven.. en registrerad användare kan se all profildata, medan anonyma endast kan se förnamnen.
#### Kör testerna från kommandoprompten
Jag använder normalt sett Resharpers Unit Test Sessions, men det går lika bra med selleri...
- Tester från kommandoprompten kör du med kommandot "dotnet test" i testprojektet ("dotnet test --help" för att se alternativ). 
- Något jag vant mig vid från tester i javascript och typescript, med [Jasmine](https://jasmine.github.io/) och [Karma](https://karma-runner.github.io), är att ha en watcher som övervakar projektfilerna och kör testerna när något ändrats, detta kan du även göra här..
- För att köra tester kontinuerligt lägger du till nugetpaketet nedan i testprojektets "tools" och kör kommandot "dotnet watch test", detta triggar alltså testrunnern automatiskt vid ändringar i testprojektet och de projekt som refereras av det.. mycket trevligt så länge du inte kör tunga integrationstester :)
	```language-json
	"tools": {
		"Microsoft.DotNet.Watcher.Tools": "1.1.0-preview4-final"
	  },
	```
## Skapa administration
Administrationssidor i MVC för innehållsadministratörerna enligt kraven 
- Som en innehållsadministratör kan jag
  - lista alla profiler
  - redigera, lägga till och ta bort profiler
