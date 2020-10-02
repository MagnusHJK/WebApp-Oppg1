###Hvordan det fungerer:
	I databasen ligger det mange stasjoner (se stasjonsliste)
	Man velger hvor man vil reise fra og til.
	Så velges dato, da sjekkes det om det finnes avganger for gitte parametere.
	Hvis de ikke eksisterer blir de automatisk generert til å gå hver andre time og til ferdigsatt pris. (Dette vil bli byttet ut med backend i oppg2)
	Så hentes de nylige genererte avgangene, etter man har valgt en avgang kan man utføre bestilling.
	Hvis man utfører bestilling blir det lagt inn, ønskes det returreise får man velge ny dato og nye avganger blir generert hvis det trengs.


###Validering:
	Det er lite input validering ettersom brukeren egentlig ikke skriver inn mye informasjon
	Dette vil det være mer av i oppg 2 når man faktisk skal endre og skrive inn informasjon