using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.ComponentModel;
// TODO: replace these with the processor input and output types.
using TInput = System.IO.FileStream;
using TOutput = MagicaVoxelContentExtension.MagicaVoxelModelData;

namespace MagicaVoxelContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "MagicaVoxelContentExtension.MagicaVoxelProcessor")]
    public class MagicaVoxelProcessor : ContentProcessor<TInput, TOutput>
    {
        private Vector3 scale = Vector3.One;

        [DisplayName("Scale")]
        [Description("Scale of the model.")]
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private Vector3 offset = Vector3.Zero;

        [DisplayName("Offset")]
        [Description("Offset of the model.")]
        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private float alpha = 1;

        [DisplayName("Alpha")]
        [Description("Alpha of the model.")]
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            return new TOutput(MagicaVoxelReader.FromMagica(input), scale, offset, alpha);
        }
    }
}