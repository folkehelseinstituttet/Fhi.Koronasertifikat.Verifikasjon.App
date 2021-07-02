<h1 align="center"> Kontroll av koronasertifikat Mobile Application <br/><img style="margin-right: 1%; margin-bottom: 1em; margin-top:1em; float: left;" src="https://raw.githubusercontent.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App/dev/FHICORC/Resources/Logo.png"> </h1>
<br/>

[![GitHub last commit](https://img.shields.io/github/last-commit/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App)](https://github.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App/commits)
[![Open pull requests](https://img.shields.io/github/issues-pr/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App)](https://github.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App/pulls)
[![Open issues](https://img.shields.io/github/issues/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App)](https://github.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App/issues)

If you are interested in backend server implementation, check out https://github.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.Backend.

## Documentation

Common questions as well as general information about Kontroll av koronasertifikat is available on [Norwegian Institute of Public Health](https://www.helsenorge.no/koronasertifikat/) (English) webpages.

## Azure Pipelines status (build and test)

|    Branch    | Status  |
|--------|---|
| main | ![Build Status](https://fhi.visualstudio.com/Fhi.Koronasertifikat.Prosjekt/_apis/build/status/prod-cd-build-deploy-appcenter?branchName=main)  
| dev    | ![Build Status](https://fhi.visualstudio.com/Fhi.Koronasertifikat.Prosjekt/_apis/build/status/prod-cd-build-deploy-appcenter?branchName=dev)

## Development
### Prerequisites
- Visual Studio 2019
- Xcode 12 or higher (iOS only)

### Getting started
1. Clone this repository using `git clone https://github.com/folkehelseinstituttet/Fhi.Koronasertifikat.Verifikasjon.App.git`
2. Open the solution file `FHICORC.sln` in Visual Studio
3. Restore Nuget Packages
4. Build the project and run it.

### Project structure
The app is written in Xamarin (C#) and platform specific UI implementation (Android XML and UIStoryboards) for additional flexibility when working with UI.
Overall, the solution contains five projects:
- **FHICORC:** Contains shared user interfaces and logic between iOS and Android, i.e., locales, models, viewModels, services.<br/><br/>
- **FHICORC.Core:** Contains shared business logic. <br/><br/>
- **FHICORC.Android:** Android related code, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **FHICORC.iOS:** iOS related code, implementation of services and handlers (for Dependency Injection) etc.<br/><br/>
- **FHICORC.Test:** Unit and integration tests.

## Contributing
Feedback and contribution are always welcome. For more information about how to contribute, refer to [Contribution Guidelines](CONTRIBUTING.md). By contributing to this project, you also agree to abide by its [Code of Conduct](CODE_OF_CONDUCT.md) at all times.

## Download Kontroll av koronasertifikat
<a href="https://play.google.com/store/apps/details?id=no.fhi.KoronasertifikatKontrollapp&gl"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/google-play-badge-en.png" width="200" height="60" alt="Get it on Google Play"></a>
<a href="https://apps.apple.com/no/app/kontroll-av-koronasertifikat/id1568677698"><img style="margin-right: 1%; margin-bottom: 0.5em; float: left;" src="https://www.helsenorge.no/globalassets/mobilapp/badges/apple-app-store-badge-en.png" width="180" height="60" alt="Download on the App Store"></a>


## Licence
Copyright (c) 2021 Norwegian Institute of Public Health (Norway), 2021 Danish Health Authority (Denmark)

Kontroll av koronasertifikat is Open Source software released under the [MIT license](LICENSE.md)



