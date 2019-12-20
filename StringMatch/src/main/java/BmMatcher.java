import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;

public class BmMatcher implements Matcher {
    public String Name() {
        return "BM";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<>();
        int bmBcA[] = getbmBc(P);
        int bmGsA[] = getbmGs(P);
        char PA[] = P.toCharArray();
        char TA[] = T.toCharArray();

        int n = T.length(), m = P.length();
        int j = 0;
        while (j <= n - m) {
            int i = m - 1;
            for (; i >= 0 && PA[i] == TA[i + j]; --i) ;
            if (i < 0) {
                result.add(j);
                i = 0;
            }
            int v1=bmGsA[i], v2=bmBcA[TA[i + j]] - m + i + 1;
            j += v1>v2? v1:v2;
        }
        return result;
    }

    public int[] getbmGs(String P) {
        int OsuffA[] = new int[P.length()];
        int bmGsA[] = new int[P.length()];

        int m = P.length(), ov;

        // Compute Osuff
        for (int i = 0; i < m; ++i) {
            OsuffA[i] = 0;
        }
        OsuffA[m - 1]=m;
        int misMatchPos = m - 1, MatchPos = m - 1;
        for (int i = m - 2; i >= 0; --i) {
            if (i > misMatchPos && (ov = OsuffA[i + m - 1 - MatchPos]) < i - misMatchPos) {
                OsuffA[i]=ov;
            } else {
                if (i < misMatchPos) {
                    misMatchPos = i;
                }
                MatchPos = i;
                while (misMatchPos >= 0 && P.charAt(misMatchPos) == P.charAt(misMatchPos + m - 1 - MatchPos))
                    --misMatchPos;
                OsuffA[i] = MatchPos - misMatchPos;
            }
        }

        // Compute bmGs
        for (int i = 0; i < m; ++i) {
            bmGsA[i] = m;
        }
        int j = 0;
        for (int i = m - 2; i >= 0; --i) {
            if (OsuffA[i] == i + 1) {
                while (j < m - 1 - i) {
                    if (bmGsA[j] == m)
                        bmGsA[j] = m - 1 - i;
                    ++j;
                }
            }
        }
        for (int i = 0; i < m - 1; ++i) {
            bmGsA[m - 1 - OsuffA[i]] = m - 1 - i;
        }
        return bmGsA;
    }

    public int[] getbmBc(String P){
        int sizeP = P.length();
        int hm[] = new int [256];
        for (int i = 0; i < 256; ++i) {
            hm[i] = sizeP;
        }
        for (int i = 0; i < sizeP - 1; ++i) {
            hm[(int) P.charAt(i)] = sizeP - i - 1;
        }
        return hm;
    }
}

