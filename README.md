Zadání:\n
Napište jednoduchý program, který bude umět detekovat změny v lokálním adresáři uvedeném na vstupu. Při prvním spuštění si program obsah daného adresáře analyzuje a při každém dalším spuštění bude hlásit změny od svého posledního spuštění, tj:\n
  a) seznam nových souborů,\n
  b) seznam změněných souborů (změnou se rozumí změna obsahu daného souboru),\n
  c) seznam odstraněných souborů a podadresářů.\n
U každého souboru evidujte číslo jeho aktuální verze (na začátku budou mít všechny soubory verzi 1, s každou detekovanou změnou daného souboru bude jeho verze navýšena o 1).\n
Program realizujte jako jednoduchou .NET Core aplikaci naprogramovanou v C#.\n
Předpokládejte, že velikost souborů v adresáři bude do 50 MB a že počet souborů v každém adresáři bude nanejvýš 100.\n
