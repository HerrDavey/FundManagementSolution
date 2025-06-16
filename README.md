<h1>Aplikacja do Zarządzania Funduszami Inwestycyjnymi 💰</h1>

<h2>Opis Projektu</h2>
Aplikacja jest systemem do zarządzania portfelem funduszy inwestycyjnych, zbudowanym w oparciu o zasady <b>Czystej Architektury (Clean Architecture)</b>. Projekt składa się z backendowego <b>API</b> napisanego w technologii ASP.NET Core Web API oraz dwóch interfejsów użytkownika: <b>Panelu Administratora</b> (Blazor Web Application) oraz <b>Aplikacji Klienckiej</b> (Blazor WebAssembly). System umożliwia kompleksowe zarządzanie funduszami, aktywami, portfelami oraz transakcjami, automatycznie przeliczając kluczowe wskaźniki, takie jak Wartość Aktywów Netto (NAV).

<h2>Główne Funkcjonalności</h2>

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

<h2>Architektura i Przepływ Danych</h2>
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

<h2>Języki i Technologie</h2>

- <b>C# 12 / .NET 8</b>
- <b>ASP.NET Core Web API</b>
- <b>Blazor Web Application </b>
- <b>Blazor WebAssembly</b>
- <b>Entity Framework Core 8</b>
- <b>SQLite</b>
- <b>MudBlazor</b> (biblioteka komponentów UI)
- <b>Serilog</b> (logowanie)
- <b>Clean Architecture</b> (wzorzec architektoniczny)

<h2>Środowisko Rozwojowe</h2>
- <b>Visual Studio 2022</b>
