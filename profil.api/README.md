# .Net core Web Api Projektanteckningar/Steps
### Projektkrav 
Applikationen ska visa en lista av konsultprofiler
- En profil består av Förnamn, Efternamn och en Beskrivning

Som en anonym användare kan jag
- Registrera mig
- Se förnamn på profilerna
	
Som en registrerad användare kan jag
- Se all information i konsultprofilerna



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
- med repositoryt [ConsultantProfileRepository.cs](ConsultantProfileRepository.cs) på plats kan vi lägga till en controller för att låta vårt api hämta data.
- för att controllern ska hittas behövs routing, det kan man sätta direkt på controllern som vi visar i koden, alternativt i Startup.cs > Configure (eller både och, controllerns konfig väger tyngst)
exempel: 
	```
	private void ConfigureRoutes(IRouteBuilder routeBuilder)
			{
				routeBuilder.MapRoute("Default",
					"{controller=Home}/{action=Index}/{id?}"); // controller=Home >> om controller name inte hittas (eller inte angavs), redirect till HomeController med default action(metod) Index
			}
	// används i Configure enligt, app.UseMvc(ConfigureRoutes)
	```


