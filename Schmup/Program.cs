using Engine.Core;
using Engine.Graphics;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using System.Drawing;

namespace Schmup;

public class SchmupGame : Application
{
    public Sprite? playerSprite;
    public Vector2D<float> playerLocation = new Vector2D<float>();
    public float playerSpeed = 300;

    public SchmupGame() : base(new Renderer2D())
    {
        Title = "Shump";
        Width = 400;
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

        Vector2D<float> direction = new Vector2D<float>();
        if(Input.IsKeyDown(Keys.W))
        {
            direction.Y = 1;
        }
        if (Input.IsKeyDown(Keys.S))
        {
            direction.Y = -1;
        }
        if (Input.IsKeyDown(Keys.A))
        {
            direction.X = -1;
        }
        if (Input.IsKeyDown(Keys.D))
        {
            direction.X = 1;
        }

        Vector2D.Normalize(direction);

        playerLocation += direction * playerSpeed * (float)time.ElapsedGametime.TotalSeconds;
    }

    public override void OnRender(GameTime time)
    {
        base.OnRender(time);

        Renderer.Submit(new RenderCommandSprite(playerSprite) { Location = playerLocation });
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
