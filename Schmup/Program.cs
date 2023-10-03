using Engine.Core;
using Engine.Graphics;
using System.Drawing;

namespace Schmup;

public class SchmupGame : Application
{
    public SchmupGame() : base(new Renderer2D())
    {
    }

    public override void OnInit()
    {
        base.OnInit();

        Renderer.SetBackgroundColor(Color.CornflowerBlue);
    }

    public override void OnUpdate(GameTime time)
    {
        base.OnUpdate(time);
    }

    public override void OnRender(GameTime time)
    {
        base.OnRender(time);

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
