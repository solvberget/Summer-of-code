namespace Solvberget.Domain.Documents
{
    class Game : Document
    {

        public new static Game GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var game = new Game();

            game.FillProperties(xml);

            return game;
        }

        public new static Game GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var game = new Game();

            game.FillPropertiesLight(xml);

            return game;
        }
    }
}
