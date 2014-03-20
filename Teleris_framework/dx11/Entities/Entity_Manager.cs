using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teleris.Components;
using Teleris.Components.Components;
using Teleris.Resources;

namespace Teleris.Entities
{
    public static class EntityManager
    {


        public static Entity Camera(string Name)
        {

            Entity e = new Entity(Name);
            CameraComponent MainCamera = new CameraComponent();

            e.Add(MainCamera);
            return e;

        }

        public static Entity Teksti(string Name)
        {

            Entity e = new Entity(Name);
            TextComponent Teksti = new TextComponent(); Teksti.Text = "Testi dataa!!!";

            e.Add(Teksti);
            return e;

        }

        public static Entity Triangle(string Name)

        {

            Entity e = new Entity(Name);
            ShaderIDComponent ShaderID = new ShaderIDComponent(); ShaderID.ShaderID = "colored.fx";
            GeometryIDComponent GeometryID = new GeometryIDComponent(); GeometryID.GeometryID = "Teapot";
            VertexComponent vertices = new VertexComponent();
            TextComponent Text = new TextComponent();
            e.Add(vertices).Add(GeometryID).Add(ShaderID).Add(Text); //Add(vertices)
            return e;
        
        }

        public static Entity CreateShip2(string Name)
        {

            Entity e = new Entity(Name);

            TextComponent text2 = new TextComponent();
            text2.Text = "Second_Entity";
            NumberComponent number2 = new NumberComponent();
            number2.Number = 56789;

            e.Add(text2).Add(number2);

            return e;


        }
    
    
    
    
    
    
    
    
    }
}
