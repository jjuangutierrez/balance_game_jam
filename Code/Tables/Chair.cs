using Godot;
using System;

public partial class Chair : Node2D
{
  [ExportGroup("NPC")]
  [Export] public Texture2D[] NpcTextures { get; private set; }
  [Export] public Sprite2D NpcSprite { get; private set; }
  [Export] public Sprite2D EmoticonSprite { get; private set; }

  [ExportGroup("Emoticons")]
  [Export] public Texture2D[] FrustrationTextures { get; private set; }
  [Export] public Texture2D HungryTexture { get; private set; }
  [Export] public Texture2D[] HappyTextures { get; private set; }

  public bool IsOccupied { get; private set; }
  public bool IsReserved { get; set; }

  Npc _currentNpc;

  public override void _Ready(){}

  public void SetNpc(Npc npc)
  {
    if (npc == null)
      throw new ArgumentNullException(nameof(npc));

    _currentNpc = npc;

    NpcSprite.Texture = NpcTextures[npc.NpcIndex];
    NpcSprite.Visible = true;

    IsOccupied = true;
    IsReserved = true;

    WaitForFood();
  }

  private async void WaitForFood()
  {
    EmoticonSprite.Texture = HungryTexture;

    bool isDishServed = false;

    const float totalWaitTime = 5.0f;
    float elapsedTime = 0f;

    while (elapsedTime < totalWaitTime)
    {
      await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
      isDishServed = _currentNpc.AssignedTable.IsDishServed(_currentNpc.ChairIndex);
      if (isDishServed)
        break;
      EmoticonSprite.Visible = true;

      await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
      EmoticonSprite.Visible = false;

      elapsedTime += 1f;
    }

    if (isDishServed)
      await ToSignal(GetTree().CreateTimer(5.0f), "timeout");

    ClearChair(isDishServed);
  }

  private void ClearChair(bool isDishServed)
  {
    _currentNpc.EmoticonSprite.Visible = true;

    if(isDishServed){
      _currentNpc.AssignedTable.ClearDish(_currentNpc.ChairIndex);
      _currentNpc.ShowHappyEmotion(HappyTextures);
    }else{
      GameManager.Instance.PlaySound("soundRage");
      _currentNpc.ShowFrustrationEmotion(FrustrationTextures);
    }

    NpcSprite.Visible = false;
    _currentNpc.Visible = true;

    _currentNpc.SetProcess(true);
    _currentNpc.MoveTo(NpcSpawnManager.Instance.ExitPoint.GlobalPosition);

    IsOccupied = false;
    IsReserved = false;
    _currentNpc = null;
  }
}