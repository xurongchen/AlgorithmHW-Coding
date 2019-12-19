import java.util.ArrayList;
import java.util.LinkedList;

public class KmpMatcher implements Matcher {
    public static ArrayList<Integer> GetPi(String P) {
        ArrayList<Integer> pi = new ArrayList<>(P.length());
        pi.add(-1);
        int matchPos = -1;
        for (int i = 1; i < P.length(); ++i) {
            while (matchPos >= 0 && P.charAt(matchPos + 1) != P.charAt(i))
                matchPos = pi.get(matchPos);
            if (P.charAt(matchPos + 1) == P.charAt(i)) {
                matchPos = matchPos + 1;
            }
            pi.add(matchPos);
        }
        return pi;
    }

    public String Name() {
        return "KMP";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<>();
        ArrayList<Integer> pi = GetPi(P);
        int matchPos = -1;
        for (int i = 1; i < T.length(); ++i) {
            while (matchPos >= 0 && P.charAt(matchPos + 1) != T.charAt(i))
                matchPos = pi.get(matchPos);

            if (P.charAt(matchPos + 1) == T.charAt(i)) {
                matchPos = matchPos + 1;
            }

            if (matchPos == P.length() - 1) {
                result.add(i - matchPos);
                matchPos = pi.get(matchPos);
            }
        }
        return result;
    }
}
