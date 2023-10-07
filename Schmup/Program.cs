using Engine.Assets;
using Engine.Core;
using Engine.Entities;
using Engine.Entities.Components;
using Engine.Graphics;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using System.Drawing;

namespace Schmup;

public class SchmupGame : Application
{
    public Sprite playerSprite;
    public Entity player;

    public Sprite pokimaneSheet;
    public SpriteAnimation pokimaneWalkUpAnimation;
    public Entity pokimane;

    public SchmupGame() : base(new Renderer2D())
    {
        Title = "Shump";
        Width = 400;
        Height = 900;
    }

    public override void OnInit()
    {
        base.OnInit();

        Renderer.SetBackgroundColor(Color.CornflowerBlue);

        playerSprite = AssetManager.Get<Sprite>("data/ship.png");
        player = Entity.Spawn(new Vector2D<float>(Width / 2 - playerSprite.Width / 2, Height / 2 - playerSprite.Height / 2), 0.0f);

        SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
        renderer.Sprite = playerSprite;

        player.AddComponent<PlayerScript>();

        pokimaneSheet = AssetManager.Get<Sprite>("data/pokimane.png");
        pokimaneWalkUpAnimation = new SpriteAnimation(pokimaneSheet, 4, 4, 0, 0, 4, 0.2f);

        pokimane = Entity.Spawn(new Vector2D<float>(100, 100), 0.0f);

        AnimatedSpriteRenderer animatedSpriteRenderer = pokimane.AddComponent<AnimatedSpriteRenderer>();
        animatedSpriteRenderer.Sprite = pokimaneWalkUpAnimation;
    }

    public override void OnUpdate(GameTime time)
    {
        base.OnUpdate(time);

        if(Input.IsKeyPressed(Keys.Escape))
        {
            Quit();
        }
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
