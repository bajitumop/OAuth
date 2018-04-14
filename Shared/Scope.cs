namespace Shared
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[Flags]
	public enum Scope
	{
		Bio = 0x1,
		Notes = 0x2,
		Images = 0x4,
	}
	
    public static class Scopes
    {
		private static readonly Dictionary<Scope, string> descriptions = new Dictionary<Scope, string>
		{
			[Scope.Bio] = "Доступ к bio",
			[Scope.Notes] = "Доступ к notes",
			[Scope.Images] = "Доступ к images",
		};
		
	    public static bool TryParse(string scopesStr, out Scope result)
	    {
		    result = default(Scope);
		    var allScopes = Enum.GetNames(typeof(Scope)).Select(x => x.ToLower());
		    var scopes = scopesStr
				.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.ToLower())
				.ToArray();

		    if (allScopes.Intersect(scopes).Count() != scopes.Length)
		    {
			    return false;
		    }

		    result = scopes.Aggregate(result, (acc, sc) => acc | (Scope) Enum.Parse(typeof(Scope), sc, true));
		    return true;

	    }

	    public static string[] ScopeDescriptions(Scope scope)
	    {
		    return scope.AsEnumerable().Select(s => descriptions[s]).ToArray();
	    }
    }

    public static class ScopeExtensions
    {
        public static IEnumerable<Scope> AsEnumerable(this Scope scope)
        {
            return Enum
                .GetValues(typeof(Scope))
                .Cast<Scope>()
                .Where(sc => (sc & scope) != 0);
        }
    }
}
