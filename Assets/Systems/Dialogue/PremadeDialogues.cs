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
        return Line.ChangeExpression(expression);
    }
    public static CharacterDialogue Template(){
        CharacterDialogue cd = new CharacterDialogue();

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
            player("Bye"),
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
    public static CharacterDialogue Florist(){
        CharacterDialogue cd = new CharacterDialogue();

        cd.entry = new Dialogue("Entry",
            player("Hello, how can I help you?"),
            cust("Hi, I want something to help me out with my garden."),
            player("Anything in particular?"),
            cust("I want my flowers to grow faster and be beautiful."),
            player("Sure, I can do that.")
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                cust("Do you like flowers?"),
                player("They're alright.")
            ),
            new Dialogue("Chat2",
                cust( "I like my garden."),
                cust( "Have you ever gardened before?"),
                player("No, i'ts not that interesting to me."),
                cust( "Oh.")
            ),
        };

        cd.exit = new Dialogue("Exit",
            player("Here you go, bye"),
            cust("Thanks! I can't wait to see my plants grow.")
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
                cust("I wanted something for my plants and you gave me... blood?"),
                player("I guess there was a mistake. Sorry about that."),
                cust("Hhmpfh")
            ),  
            new Dialogue("Fire",
                setSprite(Expression.Angry),
                cust("WHAT IS WRONG WITH YOU?"),
                player("What's the matter?"),
                cust("YOUR SPELL SET FIRE TO MY PLANTS!"),
                player("Oops lol, sorry. That happens sometimes."),
                cust("WHAT KIND OF BUSINESS IS THIS?! I'LL CONTACT THE POLICE!")
            ),    
            new Dialogue("FireWorse",
                setSprite(Expression.Angry),
                cust("WHAT IS WRONG WITH YOU?"),
                player("What's the matter?"),
                cust("THE SPELL YOU GAVE ME WAS INSANE!"),
                cust("IT DESTROYED MY WHOLE GARDEN! THE FIRE SPREAD TO MY HOUSE"),
                player("Oops lol, sorry. I hope you have insurance."),
                cust("I skimped on insurance to buy more flowers..."),
                cust("WAIT THAT'S NOT IMPORTANT I'M GONNA SUE YOU."),
                player("Good luck with that.")
            ),      
            new Dialogue("EarthPurifyLudicrous",
                cust( "The spell you gave me was really awesome."),
                cust( "Any seed I plant instantly grows big and strong."),
                player("Yep. That's magic for you."),
                setSprite(Expression.Happy),
                cust("I will be growing many cucumbers. Thank you.")
            ),
            new Dialogue("BloodNature",
                cust("So... I used the spell and..."),
                cust("My plants started asking for human blood."),
                player("Uhh... what?"),
                cust("How could I say no to my precious plants..."),
                cust("Do you do blood harvesting spells?"),
                player("I... do. But I can't give you two spells in one day."),
                cust("Ah, I guess I'll have to do it the old fashioned way."),
                player("Have fun?")
            ),      
        };

        return cd;
    }
    public static CharacterDialogue Mayor(){
        CharacterDialogue cd = new CharacterDialogue();

        cd.entry = new Dialogue("Entry",
            cust("Hello, I would like a spell to purify our city's reservoir."),
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
            new Dialogue("Perfect",
                cust("Your spell was so great! The city's water even sparkles now!"),
                player("Haha, of course I'm perfect"),
                cust("My approval will definitely go up after this, thanks!")
            ),    
            new Dialogue("Explosion",
                cust("There was an explosion, why did my spell do that?"),
                player("That happens sometimes. It's probably your fault."),
                cust("Oh... I'll have to look into that?")
            ),
            new Dialogue("ExplosionLudicrous",
                cust("THERE WAS A HUGE EXPLOSION. THE CITY'S DESTRUCTION IS IMMINENT!"),
                player("You must have messed up the casting."),
                player("The spell I gave you was definitely right."),
                cust("I... No one will ever find out.")
            ),
            new Dialogue("Blood",
                cust("This spell just poured blood into the water! Why?"),
                player("I think it has to do with your inner thoughts while casting the spell."),
                cust("What? I don't think of murdering my opponents!"),
                cust("How dare you accuse me!"),
                player("I never mentioned what thoughts exactly would cause that."),
                player("Anyways, it's your fault. Look inwards.")
            ), 
            new Dialogue("ElectricityStrong",
                cust("I think you made me kill all of the fish in our reservoir"),
                player("Yeah, its pure of sinning life now."),
                cust("I... anyways I hope to see you at the buffet.")
            ),              
            new Dialogue("WaterBlood",
                cust("WHY ON EARTH DID THE RESERVOIR TURN INTO BLOOD?!"),
                player("I think it's your fault. It might have to do with your inner thoughts."),
                cust("NO WAY! I DID EVERYTHING RIGHT. THIS IS DEFINITELY YOUR FAULT"),
                cust("YOU WILL BE INVESTIGATED IMMEDIATELY."),
                player("Are you sure you didn't make a mistake? I'ts likely you did."),
                cust("NOT THIS TIME. YOU'RE GOING DOWN.")
            ),        
        };

        return cd;
    }
    public static CharacterDialogue Prankster(){
        CharacterDialogue cd = new CharacterDialogue();

        cd.entry = new Dialogue("Entry",
            cust("Yoo."),
            player("Hello, what can I do for you?"),
            cust("I need a new spell to prank some people with."),
            cust("Bro, Just get me something harmless and funny."),
            player("Sure. That should be easy enough.")
        );
        cd.chats = new Dialogue[]{
            new Dialogue("Chat1",
                cust("Bro, I'm so excited. My tiktok is gonna blow up!"),
                player("Good for you!")
            ),
            new Dialogue("Chat2",
                player("What's up with the chicken?"),
                cust("Don't worry about it, bro.")
            ),
        };

        cd.exit = new Dialogue("Exit",
            player("Here you go. One funny spell."),
            cust("Thanks bro! I can't wait to pull an epic PRANK! HAHA.")
        );
        cd.returnings = new Dialogue[]{
            new Dialogue("Gag",
                cust( "Haha! I didn't even know fart magic was a thing!"),
                cust( "You're pretty funny, bro!"),
                player("Hehe, thanks."),
                cust("See ya!")
            ),
            new Dialogue("GagTooStrong",
                cust( "Hey... the spell was a little strong."),
                player("Oh? Is there a problem?"),
                cust( "Yeah... I think I really hurt a dude."),
                player("That's on you."),
                cust( "Not cool, bro. This is serious.")
            ), 
            new Dialogue("Blood",
                cust( "Hey, uh, bro? I wasn't really expecting to cover someone in blood."),
                player("Oh? Must have been a miscommunication."),
                cust("Yeah... my fans didn't like that one so much."),
                player("Be more specific next time.")
            ),
            new Dialogue("Electric",
                cust( "Bro! I love what you did. The shock was so unexpected and funny."),
                player("Right? I love electrocuting people!"),
                cust("I see..."),
                cust("Later, bro.")
            ),
            new Dialogue("ElectricLudicrous",
                cust("Bro, not cool."),
                cust("I just wanted to prank someone. Their heart no longer beats."),
                cust("Something so natural to us in excess causes a short circut."),
                cust("What is a prank? It's meant to make people laugh. Harmless and fun."),
                cust("However, in the pursuit of that end, tragedy may strike."),
                cust("I realize now, pranks are not a part of my future path."),
                cust("Farewell, bro."),
                player("Okay?")                
            ),
            new Dialogue("Fire",
                cust( "Uhh, shooting fire at someone isn't exactly funny."),
                player("Oh? Maybe you need a sense of humor."),
                cust("It was pretty scary, try to tone it down in the future.")
            ),
            new Dialogue("FireLudicrous",
                cust("Bro, not cool."),
                cust("I just wanted to prank someone. They're now a pile of ash."),
                cust("They were burnt to a crisp, not even their bones remained."),
                cust("What is a prank? It's meant to make people laugh. Harmless and fun."),
                cust("However, in the pursuit of that end, tragedy may strike."),
                cust("I realize now, pranks are not a part of my future path."),
                cust("Farewell, bro."),
                player("Okay?")        
            ),
            new Dialogue("Water",
                cust("Hey bro, The water was a little funny, but its kinda boring"),
                player("Oh? I thought you'd like it."),
                cust("Dude, I don't need magic to get someone dripping wet."),
                cust("Next time get me something special."),
                player("We will see in time.")
                
            ),   
            new Dialogue("GagGag",
                cust("Bro, that was the funniest shit I've ever seen."),
                cust("Like seriously, how do you even come up with it?"),
                player("I guess I'm just the best."),
                cust("Like, seriously bro, my followers absolutely love it!"),
                cust("I'm a sensation, I have a meeting with the president tomorrow!"),
                cust("Just thinking about it makes me laugh."),
                cust("hahahahahahaha"),
                cust("Okay, later, bro. Thanks again.")                
            ),             
        };

        return cd;
    }
}
