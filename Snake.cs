using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSnake
{
    public enum SnakeDirection { up, left, right, down };
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }

        public override bool Equals(object obj)
        {
            if(obj is Coordinate)
            {
                Coordinate coor = (Coordinate)obj;

                return this.x == coor.x && this.y == coor.y;
            }
            return false;
        }
    }

    public struct SnakeNode
    {
        public SnakeDirection direction;
        public Coordinate position;

        public SnakeNode(SnakeDirection dir, Coordinate coor)
        {
            direction = dir;
            position = coor;
        }
    }
    
    class Snake
    {
        ConsoleColor color;
        char character;
        bool growThisTurn;

        LinkedList<SnakeNode> snakeNodes = new LinkedList<SnakeNode>();
        Coordinate bonusPos;
        

        /// <summary>
        /// Initializes the Snake
        /// </summary>
        /// <param name="startDirection">the starting direction of the snake</param>
        /// <param name="startPos">the position of the snake head</param>
        /// <param name="startingLength">the number of snake-nodes</param>
        /// <param name="snakeColor">the color of the snake</param>
        /// <param name="snakeCharacter">the character that represents a snakeNode</param>
        public Snake(SnakeDirection startDirection, Coordinate startPos, int startingLength, ConsoleColor snakeColor, char snakeCharacter)
        {
            color = snakeColor;
            character = snakeCharacter;
            Console.ForegroundColor = color;

            for(int i = 0; i < startingLength; i++)
            {
                switch(startDirection)
                {
                    case SnakeDirection.down:
                        snakeNodes.AddLast(new SnakeNode(startDirection, new Coordinate(startPos.x, startPos.y - i)));
                        Console.SetCursorPosition(startPos.x, startPos.y - i);
                        Console.Write(character);
                        break;
                    case SnakeDirection.left:
                        snakeNodes.AddLast(new SnakeNode(startDirection, new Coordinate(startPos.x + i, startPos.y)));
                        Console.SetCursorPosition(startPos.x + i, startPos.y);
                        Console.Write(character);
                        break;
                    case SnakeDirection.right:
                        snakeNodes.AddLast(new SnakeNode(startDirection, new Coordinate(startPos.x - i, startPos.y)));
                        Console.SetCursorPosition(startPos.x - i, startPos.y);
                        Console.Write(character);
                        break;
                    case SnakeDirection.up:
                        snakeNodes.AddLast(new SnakeNode(startDirection, new Coordinate(startPos.x, startPos.y + i)));
                        Console.SetCursorPosition(startPos.x, startPos.y + i);
                        Console.Write(character);
                        break;
                }
            }

            SpawnBonus();
        }

        /// <summary>
        /// Moves the whole snake into the new direction and checks if it collides with itself or the border
        /// </summary>
        /// <param name="newDirection">The new direction of the snake</param>
        /// <returns>returns true if there is no collision and false if there is</returns>
        public bool MoveSnake(SnakeDirection newDirection)
        {
            //checks if snake grows this turn
            if (!growThisTurn)
            {
                // Deleting last node position
                Coordinate lastNodePos = snakeNodes.Last.Value.position;
                Console.SetCursorPosition(lastNodePos.x, lastNodePos.y);
                Console.Write(' ');

                // Update direction and position for each node
                var snakeNode = snakeNodes.Last;
                while(snakeNode.Previous != null)
                {
                    switch (snakeNode.Previous.Value.direction)
                    {
                        case SnakeDirection.down:
                            snakeNode.Value = new SnakeNode(snakeNode.Previous.Value.direction, 
                                new Coordinate(snakeNode.Value.position.x, snakeNode.Value.position.y + 1));
                            break;
                        case SnakeDirection.left:
                            snakeNode.Value = new SnakeNode(snakeNode.Previous.Value.direction, 
                                new Coordinate(snakeNode.Value.position.x - 1, snakeNode.Value.position.y));
                            break;
                        case SnakeDirection.right:
                            snakeNode.Value = new SnakeNode(snakeNode.Previous.Value.direction, 
                                new Coordinate(snakeNode.Value.position.x + 1, snakeNode.Value.position.y));
                            break;
                        case SnakeDirection.up:
                            snakeNode.Value = new SnakeNode(snakeNode.Previous.Value.direction, 
                                new Coordinate(snakeNode.Value.position.x, snakeNode.Value.position.y - 1));
                            break;
                    }
                    snakeNode = snakeNode.Previous;
                }
                // Update position and direction for first node
                snakeNode = snakeNodes.First;

                switch (newDirection)
                {
                    case SnakeDirection.down:
                        snakeNode.Value = new SnakeNode(newDirection, new Coordinate(snakeNode.Value.position.x, snakeNode.Value.position.y + 1));
                        break;
                    case SnakeDirection.left:
                        snakeNode.Value = new SnakeNode(newDirection, new Coordinate(snakeNode.Value.position.x - 1, snakeNode.Value.position.y));
                        break;
                    case SnakeDirection.right:
                        snakeNode.Value = new SnakeNode(newDirection, new Coordinate(snakeNode.Value.position.x + 1, snakeNode.Value.position.y));
                        break;
                    case SnakeDirection.up:
                        snakeNode.Value = new SnakeNode(newDirection, new Coordinate(snakeNode.Value.position.x, snakeNode.Value.position.y - 1));
                        break;
                }
            }
            else
            {
                // Adds a new node to the snake
                var snakeNode = snakeNodes.First.Value;
                switch (snakeNode.direction)
                {
                    case SnakeDirection.down:
                        snakeNodes.AddFirst(new SnakeNode(newDirection, new Coordinate(snakeNode.position.x, snakeNode.position.y + 1)));
                        break;
                    case SnakeDirection.left:
                        snakeNodes.AddFirst(new SnakeNode(newDirection, new Coordinate(snakeNode.position.x - 1, snakeNode.position.y)));
                        break;
                    case SnakeDirection.right:
                        snakeNodes.AddFirst(new SnakeNode(newDirection, new Coordinate(snakeNode.position.x + 1, snakeNode.position.y)));
                        break;
                    case SnakeDirection.up:
                        snakeNodes.AddFirst(new SnakeNode(newDirection, new Coordinate(snakeNode.position.x, snakeNode.position.y - 1)));
                        break;
                }

                growThisTurn = false;
            }

            // Checks if the new first Node hits a bonus or a border or the snake and prints it
            Coordinate firstNodePos = snakeNodes.First.Value.position;

            // Checks Borderhit
            if (firstNodePos.x == 10 || firstNodePos.x == 59 || firstNodePos.y == 10 || firstNodePos.y == 59)
                return false;

            // Checks Bonus hit
            if (firstNodePos.Equals(bonusPos))
            {
                growThisTurn = true;
                SpawnBonus();
            }

            // Checks snake hit
            bool first = true;
            foreach(var node in snakeNodes)
            {
                if (first)
                    first = false;
                else
                {
                    if(firstNodePos.Equals(node.position))
                    {
                        return false;
                    }
                }
            }

            // Prints new node
            Console.SetCursorPosition(firstNodePos.x, firstNodePos.y);
            Console.ForegroundColor = color;
            Console.Write(character);


            return true;
        }

        
        private void SpawnBonus()
        {
            Random rand = new Random();

            while (true)
            {
                bonusPos.x = rand.Next(11, 58);
                bonusPos.y = rand.Next(11, 58);

                foreach(var snakeNode in snakeNodes)
                {
                    if(bonusPos.Equals(snakeNode))
                        continue;
                }

                break;
            }

            Console.SetCursorPosition(bonusPos.x, bonusPos.y);
            Console.ForegroundColor = color;
            Console.Write(character);
        }
    }
}
