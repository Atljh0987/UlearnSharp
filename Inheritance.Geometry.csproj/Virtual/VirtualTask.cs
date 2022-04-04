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

        //public override RectangularCuboid GetBoundingBox()
        //{
        //    var cMin = Position;
        //    var cMax = Position;

        //    var figure = Parts[0].GetBoundingBox();

        //    double maxX = figure.Position.X + figure.SizeX / 2;
        //    double maxY = figure.Position.Y + figure.SizeY / 2;
        //    double maxZ = figure.Position.Z + figure.SizeZ / 2;

        //    double minX = figure.Position.X - figure.SizeX / 2;
        //    double minY = figure.Position.Y - figure.SizeY / 2;
        //    double minZ = figure.Position.Z - figure.SizeZ / 2;

        //    for (int i = 1; i < Parts.Count; i++)
        //    {
        //        figure = Parts[i].GetBoundingBox();
        //        double curMaxX = figure.Position.X + figure.SizeX / 2;
        //        double curMaxY = figure.Position.Y + figure.SizeY / 2;
        //        double curMaxZ = figure.Position.Z + figure.SizeZ / 2;

        //        double curMinX = figure.Position.X - figure.SizeX / 2;
        //        double curMinY = figure.Position.Y - figure.SizeY / 2;
        //        double curMinZ = figure.Position.Z - figure.SizeZ / 2;


        //        maxX = (curMaxX > maxX) ? curMaxX : maxX;
        //        minX = (curMinX < minX) ? curMinX : minX;

        //        maxY = (curMaxY > maxY) ? curMaxY : maxY;
        //        minY = (curMinY < minY) ? curMinY : minY;

        //        maxZ = (curMaxZ > maxZ) ? curMaxZ : maxZ;
        //        minZ = (curMinZ < minZ) ? curMinZ : minZ;
        //    }

        //    Vector3 center = new Vector3(2 / 2, 2 / 2, 2 / 2);

        //    return new RectangularCuboid(center, 2, 2, 2);
        //}

        public override RectangularCuboid GetBoundingBox()
        {
            var cMin = Position;
            var cMax = Position;

            for (var i = 0; i < Parts.Count; i++)
            {
                var cube = Parts[i].GetBoundingBox();
                var minPoint = GetExtremum(cube, -1);
                var maxPoint = GetExtremum(cube, 1);

                cMin = SetMinimum(minPoint, cMin);
                cMax = SetMaximum(maxPoint, cMax);
            }

            var resultVector = cMax - cMin;
            var currentPosition = new Vector3((cMin.X + cMax.X) / 2, (cMin.Y + cMax.Y) / 2, (cMin.Z + cMax.Z) / 2);
            return new RectangularCuboid(currentPosition, resultVector.X, resultVector.Y, resultVector.Z);
        }

        private Vector3 SetMinimum(Vector3 cur, Vector3 min)
        {
            if (cur.X < min.X) min = new Vector3(cur.X, min.Y, min.Z);
            if (cur.Y < min.Y) min = new Vector3(min.X, cur.Y, min.Z);
            if (cur.Z < min.Z) min = new Vector3(min.X, min.Y, cur.Z);
            return min;
        }

        private Vector3 SetMaximum(Vector3 cur, Vector3 max)
        {
            if (cur.X > max.X) max = new Vector3(cur.X, max.Y, max.Z);
            if (cur.Y > max.Y) max = new Vector3(max.X, cur.Y, max.Z);
            if (cur.Z > max.Z) max = new Vector3(max.X, max.Y, cur.Z);
            return max;
        }

        private Vector3 GetExtremum(RectangularCuboid cube, int dir)
        {
            return new Vector3(
                cube.Position.X + dir * cube.SizeX / 2,
                cube.Position.Y + dir * cube.SizeY / 2,
                cube.Position.Z + dir * cube.SizeZ / 2);
        }
    }
}