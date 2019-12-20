import java.util.LinkedList;

public class BruteForceMatcher implements Matcher {
    public String Name() {
        return "BruteForce";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<Integer>();
        char PA[] = P.toCharArray();
//        char TA[] = T.toCharArray();
        int n = T.length(), m = P.length();
        int imax = n - m;
        for (int i = 0; i < imax; ++i) {
            int j = 0;
            for (; j < m; ++j) {
                if (T.charAt(i + j) != PA[j]) {
                    break;
                }
            }
            if (j == m) result.add(i);
        }
        return result;
    }
}
