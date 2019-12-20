import java.util.LinkedList;

public class KmpMatcher implements Matcher {
    public static int[] GetPi(String P) {
        int piA[] = new int[P.length()];
        char PA[] = P.toCharArray();
        piA[0] = -1;
        int matchPos = -1, m = P.length();
        for (int i = 1; i < m; ++i) {
            while (matchPos >= 0 && PA[matchPos + 1] != PA[i])
                matchPos = piA[matchPos];
            if (PA[matchPos + 1] == PA[i]) {
                matchPos = matchPos + 1;
            }
            piA[i] = matchPos;
        }
        return piA;
    }

    public String Name() {
        return "KMP";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<>();
        int[] piA = GetPi(P);
        char PA[] = P.toCharArray();
        int matchPos = -1,m = P.length(),n=T.length();
        for (int i = 1; i < n; ++i) {
            while (matchPos >= 0 && PA[matchPos + 1] != T.charAt(i))
                matchPos = piA[matchPos];
            if (PA[matchPos + 1] == T.charAt(i)) {
                matchPos = matchPos + 1;
            }
            if (matchPos == m - 1) {
                result.add(i - matchPos);
                matchPos = piA[matchPos];
            }
        }
        return result;
    }
}
