using System;
using System.Collections.Generic;

/* NOTE:
 *
 * Because the tax rule is the same for all items, I implemented 
 * GetFullPrice() on the base class instead of forcing subclasses
 * to override it. Item remains abstract because it 
 * represents a base type that should not be instantiated.
 *
 * Since the question asks that each item must have a price and weight
 * and repeating the same initialization code for each Item felt unnecessary,
 * I made a base constructor on Item.
 */

public class Program
{
    public static void Main()
    {

        //items contains all the items to buy
        List<Item> items = new List<Item>();

        //add the table, paddle and balls with the required quantities
        items.Add(new Table(100, 1));

        items.Add(new Paddle(50, 0.5f));
        items.Add(new Paddle(50, 0.5f));

        items.Add(new Balls(20, 0.5f));
        items.Add(new Balls(20, 0.5f));
        items.Add(new Balls(20, 0.5f));

        //Display the name of each item in items
        foreach (Item item in items)
        {
            Console.WriteLine("{0}\t:\t${1:F2}", item.GetType().Name, item.GetFullPrice());
        }
    }
}

public abstract class Item
{
    protected int price;
    protected float weight;

    protected Item(int price, float weight)
    {
        this.price = price;
        this.weight = weight;
    }

    public float GetFullPrice()
    {
        return price * 1.2f;
    }

    public float Weight { get { return weight; } }
}

public class Table : Item
{
    public Table(int price, float weight) : base(price, weight) { }
}

public class Paddle : Item
{
    public Paddle(int price, float weight) : base(price, weight) { }
}

public class Balls : Item
{
    public Balls(int price, float weight) : base(price, weight) { }
}
