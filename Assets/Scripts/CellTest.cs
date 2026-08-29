using System;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
public class CellTest : MonoBehaviour
{
   [SerializeField] public Transform targetStack;
   public float jumpHeight = 2f;
   public float moveDuration = 0.5f; 
   private Vector3 originPosition;

   private void Start()
   {
      originPosition = this.transform.position;
   }

   [Button]
   private void GoToTarget()
   {
      // 1. Tạo Sequence để gom các hành động lại
      Sequence jumpSeq = DOTween.Sequence();
      Vector3 targetPosition = targetStack.position;
      // 2. Hành động 1: Bay theo đường cong Parabol
      // DOJump(điểm đích, độ cao, số lần nhảy, thời gian)
      // Dùng Ease.OutQuad để mảnh ném lên dứt khoát và rơi xuống có cảm giác trọng lực
      jumpSeq.Append(transform.DOJump(targetPosition.With(y:targetPosition.y + .2f), jumpHeight, 1, moveDuration)
         .SetEase(Ease.OutQuad));

      // 3. Hành động 2: Lật mặt mảnh ghép trong không trung
      // RotateMode.LocalAxisAdd rất quan trọng ở đây: Nó sẽ cộng thêm 180 độ vào góc xoay hiện tại
      // Đảm bảo mảnh ghép luôn lật đúng 1 nửa vòng dù trước đó nó đang nằm ở góc nào.
      jumpSeq.Join(transform.DORotate(new Vector3(180f, 90, 0), moveDuration, RotateMode.LocalAxisAdd)
         .SetEase(Ease.Linear));

      // 4. Hành động 3: Hiệu ứng "Juicy" khi chạm đất (Squash & Stretch)
      jumpSeq.OnComplete(() => {
         // Khi bay xong, nén trục Y xuống (bẹp lại) và phình trục X, Z ra một chút
         // Các tham số: (Độ biến dạng, Thời gian nảy, Số lần rung)
         transform.DOPunchScale(new Vector3(0.1f, -0.2f, 0.1f), 0.15f, 1);
            
         // Ở đây bạn có thể gọi thêm Audio Manager để phát tiếng "Cộc" khi chạm đất
      });
   }
   [Button]
   private void GoBack()
   {
      transform.DOMove(originPosition, 1f).SetEase(Ease.Flash);
      transform.DORotate(new Vector3(0, 0, 0),  moveDuration, RotateMode.FastBeyond360)
         .SetEase(Ease.Linear);
   }
}
