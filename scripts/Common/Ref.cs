/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
namespace StompyBlondie.Common
{
	/**
	 * A class that can hold a reference to any type of object. Useful for some obscure scenarios.
	 */
	public class Ref<T>
	{
	    private T store;

	    public T Value{
	        get { return store; }
	        set { store = value; }
	    }

	    public Ref(T reference)
	    {
	        store = reference;
	    }
	}

}