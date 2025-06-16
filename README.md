<h1>Aplikacja do Zarządzania Funduszami Inwestycyjnymi 🏦</h1>

<h2>1. Opis Projektu</h2>

Aplikacja jest kompleksowym systemem do zarządzania portfelem funduszy inwestycyjnych. Została zaprojektowana i zaimplementowana w oparciu o zasady **Czystej Architektury (Clean Architecture)**, aby zapewnić elastyczność, testowalność i wyraźne rozdzielenie logiki biznesowej od warstwy prezentacji i dostępu do danych.

System składa się z następujących, kluczowych komponentów:

* **Backend (ASP.NET Core Web API):** Stanowi centralny punkt aplikacji, który wystawia bezpieczne RESTful API i zawiera całą logikę biznesową oraz operacje na danych.

* **Panel Administratora (Blazor Web Application):** Interfejs typu "back-office" przeznaczony dla zaawansowanego użytkownika. Posiada pełne uprawnienia (CRUD) do zarządzania podstawowymi danymi systemu, takimi jak fundusze i dostępne aktywa, a także do wglądu we wszystkie portfele i transakcje.

* **Aplikacja Kliencka (Blazor WebAssembly):** Lekki interfejs typu "front-office" działający w całości w przeglądarce użytkownika. Jest przeznaczony dla analityka lub inwestora i skoncentrowany na przeglądaniu szczegółów konkretnych portfeli i wykonywaniu transakcji.

Kluczową funkcją systemu jest **automatyczne przeliczanie Wartości Aktywów Netto (NAV)** portfela po każdej operacji (kupno, sprzedaż, edycja, usunięcie), co odzwierciedla realne procesy księgowości funduszy.

<h2>2. Instalacja i Uruchomienie</h2>

### Wymagania
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Kroki
1. Sklonuj repozytorium na swój dysk lokalny.
2. Otwórz plik solucji (`.sln`) w Visual Studio 2022.
3. **Konfiguracja CORS**: W projekcie `Fundusze.WebAPI`, w pliku `Program.cs`, znajdź linię `policy.WithOrigins("...")` i upewnij się, że port jest zgodny z tym, na którym uruchamia się projekt `Fundusze.Client` (można to sprawdzić w `Fundusze.Client/Properties/launchSettings.json`).
4. **Konfiguracja Projektów Startowych**:
    - Kliknij prawym przyciskiem myszy na solucję w `Solution Explorer`.
    - Wybierz `Configure Startup Projects...`.
    - Zaznacz `Multiple startup projects`.
    - Dla projektów `Fundusze.WebAPI`, `Fundusze.BlazorAppServer` oraz `Fundusze.Client` ustaw `Action` na `Start`.
5. Naciśnij klawisz `F5` lub przycisk `Start`, aby uruchomić wszystkie trzy aplikacje jednocześnie. Aplikacja utworzy bazę danych `fundusze.db` przy pierwszym uruchomieniu.

<h2>3. Główne Funkcjonalności</h2>

- <b>Czysta Architektura</b>: Jasny podział na warstwy (Domena, Aplikacja, Infrastruktura, Prezentacja) w celu zapewnienia elastyczności, testowalności i łatwości w utrzymaniu kodu.

- <b>Dwa Interfejsy Użytkownika</b>:
    - <b>Panel Administratora (Blazor Web Application)</b>: Posiada pełne uprawnienia do tworzenia, edycji i usuwania wszystkich danych w systemie (CRUD).
    - <b>Aplikacja Kliencka (Blazor WebAssembly)</b>: Skoncentrowana na przeglądaniu danych i dokonywaniu transakcji z perspektywy klienta/inwestora.

- <b>Logika Biznesowa</b>: Aplikacja zawiera zaawansowaną logikę biznesową, m.in.:
    - Automatyczne przeliczanie wartości portfela (NAV) po każdej transakcji (kupno, sprzedaż, edycja, usunięcie).
    - Dynamiczne filtrowanie dostępnych aktywów w zależności od typu funduszu.
    - Zabezpieczenia integralności danych (np. blokowanie usunięcia funduszu, który posiada portfele).

- <b>API RESTful</b>: Backend wystawia w pełni funkcjonalne API do komunikacji z interfejsami użytkownika.

- <b>Baza Danych</b>: System wykorzystuje lekką bazę danych SQLite zarządzaną poprzez Entity Framework Core.

<h2>4. Architektura i Przepływ Danych</h2>
Aplikacja została zbudowana zgodnie z zasadami Clean Architecture, co zapewnia separację logiki biznesowej od szczegółów implementacyjnych.

- <b>Fundusze.Domain</b>: Serce aplikacji. Zawiera encje biznesowe (np. `Fund`, `Asset`) oraz interfejsy repozytoriów. Nie ma żadnych zależności.
- <b>Fundusze.Application</b>: Zawiera logikę aplikacji (serwisy), która orkiestruje operacje na encjach. Definiuje przypadki użycia systemu. Zależy tylko od warstwy Domeny.
- <b>Fundusze.Infrastructure</b>: Implementuje interfejsy z warstw wewnętrznych. To tutaj znajduje się konfiguracja bazy danych (DbContext) oraz implementacje repozytoriów.
- <b>Fundusze.WebAPI</b>: Warstwa prezentacji, która wystawia funkcjonalność aplikacji światu poprzez kontrolery API.
- <b>Fundusze.BlazorAppServer / Fundusze.Client</b>: Interfejsy użytkownika, które komunikują się wyłącznie z WebAPI.

<b>Przykładowy przepływ:</b>
1. Użytkownik w aplikacji Blazor klika "Dodaj Transakcję".
2. Interfejs użytkownika wysyła żądanie `POST` do `TransactionController` w WebAPI.
3. Kontroler wywołuje metodę w `TransactionService` (warstwa Aplikacji).
4. Serwis wykorzystuje encje z Domeny do przetworzenia logiki (np. obliczenia NAV) i wywołuje metody na interfejsach repozytoriów.
5. `TransactionRepository` (warstwa Infrastruktury) wykonuje operacje na bazie danych.
6. Odpowiedź wraca tą samą drogą do użytkownika.

<h2>5. Przegląd Kluczowych Endpointów API</h2>
Pełna, interaktywna dokumentacja API jest dostępna poprzez Swaggera po uruchomieniu projektu WebAPI. Poniżej znajdują się najważniejsze endpointy:

- `POST /api/transaction` - Tworzy nową transakcję i automatycznie przelicza wartość (NAV) powiązanego portfela.
- `DELETE /api/transaction/{id}` - Usuwa transakcję i cofa jej wpływ na NAV portfela.
- `GET /api/portfolio/{id}/details` - Zwraca szczegółowe, zagregowane dane dla konkretnego portfela, włączając w to listę posiadanych aktywów i pełną historię transakcji.
- `GET /api/fund`, `POST /api/fund` itd. - Pełne operacje CRUD do zarządzania funduszami.

<h2>6. Języki i Technologie</h2>

- <b>C# 12 / .NET 8</b>
- <b>ASP.NET Core Web API</b>
- <b>Blazor Web Application </b>
- <b>Blazor WebAssembly</b>
- <b>Entity Framework Core 8</b>
- <b>SQLite</b>
- <b>MudBlazor</b> (biblioteka komponentów UI)
- <b>Serilog</b> (logowanie)
- <b>Clean Architecture</b> (wzorzec architektoniczny)

<h2>7. Środowisko</h2>

- <b>Visual Studio 2022</b>
