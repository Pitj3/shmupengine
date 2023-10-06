using Engine.Assets;
using Engine.Core;
using Engine.Graphics;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using System.Drawing;

namespace Schmup;

public class SchmupGame : Application
{
    public Sprite playerSprite;
    public Vector2D<float> playerLocation;
    public float playerSpeed = 300;

    public Sprite playerSheet;
    public SpriteAnimation animatedPlayer;

    public SchmupGame() : base(new Renderer2D())
    {
        Title = "Shump";
        Width = 400;
        Height = 900;
    }

    public override void OnInit()
    {
        base.OnInit();

        playerSprite = AssetManager.Get<Sprite>("data/ship.png");
        playerLocation = new Vector2D<float>(Width / 2 - playerSprite.Width / 2, Height / 2 - playerSprite.Height / 2);

        Renderer.SetBackgroundColor(Color.CornflowerBlue);

        playerSheet = AssetManager.Get<Sprite>("data/pokimane.png");

        animatedPlayer = new SpriteAnimation(playerSheet, 4, 4, 0, 0, 4, 0.1f);
    }

    public override void OnUpdate(GameTime time)
    {
        base.OnUpdate(time);

        if(Input.IsKeyPressed(Keys.Escape))
        {
            Quit();
        }

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

        playerLocation += direction * playerSpeed * time.Delta;

        playerLocation.X = Math.Clamp(playerLocation.X, 0, Width - playerSprite.Width);
        playerLocation.Y = Math.Clamp(playerLocation.Y, 0, Height - playerSprite.Height); 
    }

    public override void OnRender(GameTime time)
    {
        base.OnRender(time);

        Renderer.Submit(new RenderCommandAnimatedSprite(animatedPlayer) { Location = playerLocation, Scale = Vector2D<float>.One });
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
