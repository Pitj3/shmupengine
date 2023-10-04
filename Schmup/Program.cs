using Engine.Core;
using Engine.Graphics;
using Silk.NET.Maths;
using System.Drawing;

namespace Schmup;

public class SchmupGame : Application
{
    public Sprite? playerSprite;

    public SchmupGame() : base(new Renderer2D())
    {
        Title = "Shump";
        Width = 900;
        Height = 900;
    }

    public override void OnInit()
    {
        base.OnInit();

        playerSprite = new Sprite("data/ship.png");

        Renderer.SetBackgroundColor(Color.CornflowerBlue);
    }

    public override void OnUpdate(GameTime time)
    {
        base.OnUpdate(time);
    }

    public override void OnRender(GameTime time)
    {
        base.OnRender(time);

        Renderer.Submit(new RenderCommandSprite(playerSprite) { Location = new Vector2D<float>(0, 0) });
    }
}

public class Program
{
    public static void Main(string[] _)
    {
        SchmupGame game = new();
        game.Run();
    }
}
