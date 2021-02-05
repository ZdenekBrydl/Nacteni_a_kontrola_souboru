Zadání:

Napište jednoduchý program, který bude umět detekovat změny v lokálním adresáři uvedeném na vstupu. Při prvním spuštění si program obsah daného adresáře analyzuje a při každém dalším spuštění bude hlásit změny od svého posledního spuštění, tj:

  a) seznam nových souborů,
  
  b) seznam změněných souborů (změnou se rozumí změna obsahu daného souboru),
  
  c) seznam odstraněných souborů a podadresářů.
  
U každého souboru evidujte číslo jeho aktuální verze (na začátku budou mít všechny soubory verzi 1, s každou detekovanou změnou daného souboru bude jeho verze navýšena o 1).

Program realizujte jako jednoduchou .NET Core aplikaci naprogramovanou v C#.

Předpokládejte, že velikost souborů v adresáři bude do 50 MB a že počet souborů v každém adresáři bude nanejvýš 100.
