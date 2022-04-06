using System.Collections.Generic;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor
    {
        Body Visit(Ball ball);
        Body Visit(RectangularCuboid rectangularCuboid);
        Body Visit(Cylinder cylinder);
        Body Visit(CompoundBody compoundBody);
    }

    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract RectangularCuboid GetBoundingBox();

        public abstract Body Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }
        public override RectangularCuboid GetBoundingBox()
        {
            double length = Radius * 2;
            return new RectangularCuboid(Position, length, length, length);
        }

        public override Body Accept(IVisitor visitor) => visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var figure = Parts[0].GetBoundingBox();

            double maxX = figure.Position.X + figure.SizeX / 2;
            double maxY = figure.Position.Y + figure.SizeY / 2;
            double maxZ = figure.Position.Z + figure.SizeZ / 2;

            double minX = figure.Position.X - figure.SizeX / 2;
            double minY = figure.Position.Y - figure.SizeY / 2;
            double minZ = figure.Position.Z - figure.SizeZ / 2;

            for (int i = 1; i < Parts.Count; i++)
            {
                figure = Parts[i].GetBoundingBox();
                double curMaxX = figure.Position.X + figure.SizeX / 2;
                double curMaxY = figure.Position.Y + figure.SizeY / 2;
                double curMaxZ = figure.Position.Z + figure.SizeZ / 2;

                double curMinX = figure.Position.X - figure.SizeX / 2;
                double curMinY = figure.Position.Y - figure.SizeY / 2;
                double curMinZ = figure.Position.Z - figure.SizeZ / 2;


                maxX = (curMaxX > maxX) ? curMaxX : maxX;
                minX = (curMinX < minX) ? curMinX : minX;

                maxY = (curMaxY > maxY) ? curMaxY : maxY;
                minY = (curMinY < minY) ? curMinY : minY;

                maxZ = (curMaxZ > maxZ) ? curMaxZ : maxZ;
                minZ = (curMinZ < minZ) ? curMinZ : minZ;
            }

            var resX = maxX - minX;
            var resY = maxY - minY;
            var resZ = maxZ - minZ;

            Vector3 center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);

            return new RectangularCuboid(center, resX, resY, resZ);
        }

        public CompoundBody ConvertToRectangulars()
        {
            List<Body> parts = new List<Body>();

            foreach(var el in Parts)
            {
                if(el is RectangularCuboid)
                {
                    parts.Add(el);
                } 
                else if (el is CompoundBody cp)
                {
                    cp = cp.ConvertToRectangulars();
                    parts.Add(cp);
                }
                else
                {
                    parts.Add(el.GetBoundingBox());
                }
            }

            return new CompoundBody(parts);
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball ball) => ball.GetBoundingBox();
        public Body Visit(RectangularCuboid rc) => rc.GetBoundingBox();

        public Body Visit(Cylinder cylinder) => cylinder.GetBoundingBox();

        public Body Visit(CompoundBody compoundBody) => compoundBody.GetBoundingBox();
    }

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball ball) => ball.GetBoundingBox();
        public Body Visit(RectangularCuboid rc) => rc.GetBoundingBox();

        public Body Visit(Cylinder cylinder) => cylinder.GetBoundingBox();

        public Body Visit(CompoundBody compoundBody) => compoundBody.ConvertToRectangulars();
    }
}