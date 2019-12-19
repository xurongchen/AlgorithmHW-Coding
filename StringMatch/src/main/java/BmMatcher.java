import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;

public class BmMatcher implements Matcher {
    public String Name() {
        return "BM";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<>();
        BMBC bmBc = new BMBC(P);
        ArrayList<Integer> bmGs = getbmGs(P);
        int n = T.length(), m = P.length();
        int j = 0;
        while (j <= n - m) {
            int i = m - 1;
            for (; i >= 0 && P.charAt(i) == T.charAt(i + j); --i) ;
            if (i < 0) {
                result.add(j);
                i = 0;
            }
            j += Math.max(bmGs.get(i), bmBc.get(T.charAt(i + j)) - m + i + 1);
        }
        return result;
    }

    public ArrayList<Integer> getbmGs(String P) {
        ArrayList<Integer> Osuff = new ArrayList<>(P.length());
        ArrayList<Integer> bmGs = new ArrayList<>(P.length());

        int m = P.length();

        // Compute Osuff
        for (int i = 0; i < m; ++i) {
            Osuff.add(0);
        }
        Osuff.set(m - 1, m);
        int misMatchPos = m - 1, MatchPos = m - 1;
        for (int i = m - 2; i >= 0; --i) {
            if (i > misMatchPos && Osuff.get(i + m - 1 - MatchPos) < i - misMatchPos) {
                Osuff.set(i, Osuff.get(i + m - 1 - MatchPos));
            } else {
                if (i < misMatchPos) {
                    misMatchPos = i;
                }
                MatchPos = i;
                while (misMatchPos >= 0 && P.charAt(misMatchPos) == P.charAt(misMatchPos + m - 1 - MatchPos))
                    --misMatchPos;
                Osuff.set(i, MatchPos - misMatchPos);
            }
        }

        // Compute bmGs
        for (int i = 0; i < m; ++i) {
            bmGs.add(m);
        }
        int j = 0;
        for (int i = m - 2; i >= 0; --i) {
            if (Osuff.get(i) == i + 1) {
                while (j < m - 1 - i) {
                    if (bmGs.get(j) == m)
                        bmGs.set(j, m - 1 - i);
                    ++j;
                }
            }
        }
        for (int i = 0; i < m - 1; ++i) {
            bmGs.set(m - 1 - Osuff.get(i), m - 1 - i);
        }
        return bmGs;
    }

    public static class BMBC {
        HashMap<Character, Integer> hm;
        int sizeP = 0;

        public BMBC(String P) {
            hm = new HashMap<>();
            sizeP = P.length();
            for (int i = 0; i < sizeP - 1; ++i) {
                hm.put(P.charAt(i), sizeP - i - 1);
            }
        }

        public Integer get(Character c) {
            if (hm.containsKey(c)) return hm.get(c);
            else return sizeP;
        }
    }
}

