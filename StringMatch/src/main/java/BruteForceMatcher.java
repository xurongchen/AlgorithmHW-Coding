import java.util.LinkedList;

public class BruteForceMatcher implements Matcher {
    public String Name() {
        return "BruteForce";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<Integer>();
        int imax = T.length() - P.length();
        for (int i = 0; i < imax; ++i) {
            int j = 0;
            for (; j < P.length(); ++j) {
                if (T.charAt(i + j) != P.charAt(j)) {
                    break;
                }
            }
            if (j == P.length()) result.add(i);
        }
        return result;
    }
}
