# Temat: Sklep internetowy

## Funkcjonalności
- Logowanie, 
- Przeglądanie dostepnych produktów, 
- Dodawanie produktów do koszyka, przeglądanie oraz możliwość kasowania,
- Finalizacja zamówienia,
- ADMIN: Dodawanie/Usuwanie/Edytowanie produktów
- ADMIN: Dodawanie/Usuwanie/Edytowanie kategorii
- ADMIN: Podgląd na historię zamówień wraz ze szczegółami

## Instalacja projektu
### Wymagane oprogramowanie
Visual Studio 2019/2022
Ms SQL Server
### Instalacja
1. Uruchamiamy Ms SQL Server i kopiujemy nasz "server name"
2. Uruchamiamy plik OnlineStore.sln w VS
3. Otwieramy plik appsettings.json i w 3 linijce w miejscu "Server" wklejamy naszą nazwe serwera.
4. Wybieramy Narzędzia->Menedżer Pakietów NuGet -> Konsola menedżera pakietów
5. W konsole wpisujemy 2 komendy:
   Add-Migration "Initial Create"
   Update-Database

Wbudowane konto ADMINA:
- Login: admin@example.com 
- Hasło: 123456

Wbudowane konto USERA:
- Login: user@example.com 
- Hasło: password
