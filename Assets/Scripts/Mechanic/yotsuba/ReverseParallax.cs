using UnityEngine;

namespace Mechanic.yotsuba
{
  public class ReverseParallax : Parallax
  {
    protected override void SetPosition(float wrap)
    {
        transform.position = new Vector3(startX - wrap, transform.position.y, transform.position.z);
    }
  }
}