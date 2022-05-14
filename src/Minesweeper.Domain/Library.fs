namespace Minesweeper.Domain

type Cell =
   | Covered of Cell
   | Number of int 
   | Bomb
    
