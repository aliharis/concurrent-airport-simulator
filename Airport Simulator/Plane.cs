
using System;
using System.Drawing;

namespace Airport_Simulator
{
    public class Plane
    {
        private Color color;
        private Point position;
        private int destination; 

        public Plane(Point position, int destination, Color color)
        {
            this.position = position;
            this.destination = destination;
            this.color = color;
        }

        public Color getColor()
        {
            return this.color;
        }

        public int getDestination()
        {
            return this.destination;
        }

        public int getPositionX()
        {
            return this.position.X;
        }

        public int getPositionY()
        {
            return this.position.Y;
        }

        public void setPosition(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void setDestination(int destination)
        {
            this.destination = destination;
        }

        public void movePlane(int xDelta, int yDelta)
        {
            this.position.X += xDelta;
            this.position.Y += yDelta;
        }
    }
}
