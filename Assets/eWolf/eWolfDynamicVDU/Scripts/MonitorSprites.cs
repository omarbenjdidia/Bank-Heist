using eWolf.eWolfDynamicVDU.Scripts.Data;
using eWolf.eWolfDynamicVDU.Scripts.Interfaces;
using UnityEngine;

namespace eWolf.eWolfDynamicVDU.Scripts
{
    public class MonitorSprites : MonitorScreens
    {
        public Texture2D Texture;

        public override void Start()
        {
            _delay = UnityEngine.Random.Range(5, 30);
            _pixelsHolder = new TexturePixelsHolder(GetComponent<Sprite>(), Texture);
            _pixelsHolder.FillBackGroundColor(ColorSets.BackGround);

            _effects.AddRange(AddEffects());

            DrawAllBoarders();

            foreach (IModifierEffects effect in _effects)
            {
                if (!IsPaused)
                {
                    effect.StartEffect(Alert);
                }
            }
        }
    }
}