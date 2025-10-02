using System;
using Ucu.Poo.Twitter;

namespace Library
{
    public abstract class BaseRepo
    {
        public void TweetPhoto()
        {
            var twitter = new TwitterImage();
            Console.WriteLine(twitter.PublishToTwitter("text", @"PathToImage.png"));
        }
    }
}