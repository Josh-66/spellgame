using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PremadeDialogues{
    public static Line cust(string t){
        return new Line(true,t);
    }
    public static Line player(string t){
        return new Line(false,t);
    }
    public static Line setSprite(Expression expression){
        return new Line (true,"",true){expression=expression,actionType=1};
    }
    public static CharacterDialouge Template(){
        CharacterDialouge cd = new CharacterDialouge();

        cd.entry = new Dialogue("Entry",
            cust("Hello"),
            player("Hello")
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                cust("Chat"),
                player("Yay")

            ),
            new Dialogue("Chat2",
                cust( "More"),
                player("Yay again!")
            ),
        };

        cd.exit = new Dialogue("Exit",
            player("Here"),
            cust("Cool")
        );
        cd.returnings = new Dialogue[]{
            new Dialogue("UsedFire",
                cust( "Why?"),
                player("Oops lol")
            ),
            new Dialogue("TooWeak",
                cust( "its not great"),
                player("next time")
            ),            
        };

        return cd;
    }
    public static CharacterDialouge Florist(){
        CharacterDialouge cd = new CharacterDialouge();

        cd.entry = new Dialogue("Entry",
            cust("Hello"),
            player("What do you want?"),
            cust("Flower powers"),
            player("Sure, I can do that. Just wait a momento"),
            cust("Thanks so much! Youre my favorite person in the world"),
            cust("I feel like i can trust you so much"),
            setSprite(Expression.Happy)
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                setSprite(Expression.Angry),
                cust( "You make me sick"),
                player("Great! I have a spell to fix that")

            ),
            new Dialogue("Chat2",
                cust( "I like my garden"),
                player("I sure hope nothing happens to it")
            ),
        };

        cd.exit = new Dialogue("Exit",
            player("Here you go, bye"),
            cust("Thanks!")
        );
        cd.returnings = new Dialogue[]{
            new Dialogue("Perfect",
                cust( "Hello! I just wanted to let you know that the spell you gave to me was perfect! Thank you so much!"),
                player("No problem! I hope to see you again sometime."),
                cust("I'll be sure to be back! And recommend you to all my friends!")
            ),
            new Dialogue("Blood",
                setSprite(Expression.Angry),
                cust("Excuse me! Is this some kind of joke?"),
                player("What's the matter?"),
                cust("I wanted something for my plants and you give blood?"),
                player("I guess there was a mistake. Sorry about that."),
                cust("Hhmpfh")
            ),  
            new Dialogue("UsedFire",
                setSprite(Expression.Angry),
                cust("WHAT IS WRONG WITH YOU?"),
                player("What's the matter?"),
                cust("YOUR SPELL SET FIRE TO MY PLANTS!"),
                player("Oops lol, sorry. That happens sometimes."),
                cust("WHAT KIND OF BUSINESS IS THIS, I'LL CONTACT THE POLICE!")
            ),    
            new Dialogue("UsedFireWorse",
                setSprite(Expression.Angry),
                cust("WHAT IS WRONG WITH YOU?"),
                player("What's the matter?"),
                cust("THE SPELL YOU GAVE ME WAS INSANE!"),
                cust("IT DESTROYED MY WHOLE GARDEN! THE FIRE SPREAD TO MY HOUSE"),
                player("Oops lol, sorry. That happens sometimes."),
                cust("WHAT KIND OF BUSINESS IS THIS, I'LL CONTACT THE POLICE!")
            ),            
        };

        return cd;
    }
    public static CharacterDialouge Mayor(){
        CharacterDialouge cd = new CharacterDialouge();

        cd.entry = new Dialogue("Entry",
            cust("Hello, I would like a spell to purify the city's reservoir."),
            player("Okay sure I'll do that.")
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                cust("I can't believe there was a mass poisioning yesterday. Who would do that to us?"),
                player("Yeah... so strange.")

            ),
            new Dialogue("Chat2",
                cust("Did you vote for me? I bet you did! I'm great."),
                player("I don't really care about that.")
            ),
        };
        cd.exit = new Dialogue("Exit",
            player("Here you are."),
            cust("Thank you! I will use this to help the city immediately.")
        );
        cd.returnings = new Dialogue[]{
            new Dialogue("Explosion",
                cust("There was an explosion, why did my spell do that?"),
                player("Oops lol")
            ),
            new Dialogue("ExplosionLudicrous",
                cust("THERE WAS A HUGE EXPLOSION. THE CITY'S DESTRUCTION IS IMMINENT!"),
                player("Oops lol")
            ),
            new Dialogue("Perfect",
                cust("Your spell was so great! The city's water even sparkles now!"),
                player("Haha, of course I'm perfect"),
                cust("My approval will definitely go up after this, thanks!")
            ),    
            new Dialogue("Blood",
                cust("This spell just poured blood into the water! What the fuck?"),
                player("Haha get rekt.")
            ),        
        };

        return cd;
    }
    public static CharacterDialouge Prankster(){
        CharacterDialouge cd = new CharacterDialouge();

        cd.entry = new Dialogue("Entry",
            cust("Hello"),
            player("Hello")
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                cust("Chat"),
                player("Yay")

            ),
            new Dialogue("Chat2",
                cust( "More"),
                player("Yay again!")
            ),
        };

        cd.exit = new Dialogue("Exit",
            player("Here"),
            cust("Cool")
        );
        cd.returnings = new Dialogue[]{
            new Dialogue("UsedFire",
                cust( "Why?"),
                player("Oops lol")
            ),
            new Dialogue("TooWeak",
                cust( "its not great"),
                player("next time")
            ),            
        };

        return cd;
    }
}
