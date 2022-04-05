using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            double length = Radius * 2;
            return new RectangularCuboid(Position, length, length, length);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                    Position.X - SizeX / 2,
                    Position.Y - SizeY / 2,
                    Position.Z - SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + SizeX / 2,
                Position.Y + SizeY / 2,
                Position.Z + SizeZ / 2);

            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, SizeX, SizeY, SizeZ);
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - SizeZ / 2;
            var maxZ = minZ + SizeZ;

            return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            double length = Radius * 2;
            return new RectangularCuboid(Position, length, length, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var posMin = Position;
            var posMax = Position;

            for (int i = 0; i < Parts.Count; i++)
            {
                var figure = Parts[i].GetBoundingBox();

                var vectorMax = new Vector3(
                    figure.Position.X + figure.SizeX / 2,
                    figure.Position.Y + figure.SizeY / 2,
                    figure.Position.Z + figure.SizeZ / 2
                );

                var vectorMin = new Vector3(
                    figure.Position.X - figure.SizeX / 2,
                    figure.Position.Y - figure.SizeY / 2,
                    figure.Position.Z - figure.SizeZ / 2
                );


                posMin = new Vector3(
                    (vectorMin.X < posMin.X) ? vectorMin.X : posMin.X,
                    (vectorMin.Y < posMin.Y) ? vectorMin.Y : posMin.Y,
                    (vectorMin.Z < posMin.Z) ? vectorMin.Z : posMin.Z
                );

                posMax = new Vector3(
                    (vectorMax.X > posMax.X) ? vectorMax.X : posMax.X,
                    (vectorMax.Y > posMax.Y) ? vectorMax.Y : posMax.Y,
                    (vectorMax.Z > posMax.Z) ? vectorMax.Z : posMax.Z
                );
            }

            var resultVector = posMax - posMin;
            var currentPosition = new Vector3((posMin.X + posMax.X) / 2, (posMin.Y + posMax.Y) / 2, (posMin.Z + posMax.Z) / 2);
            return new RectangularCuboid(currentPosition, resultVector.X, resultVector.Y, resultVector.Z);
        }
    }
}