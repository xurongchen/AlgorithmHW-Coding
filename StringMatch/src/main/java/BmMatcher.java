import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;

public class BmMatcher implements Matcher {
    public String Name() {
        return "BM";
    }

    public LinkedList<Integer> Match(String T, String P) {
        LinkedList<Integer> result = new LinkedList<>();
//        BMBC bmBc = new BMBC(P);
//        int bmBcA[] = bmBc.hm;
        int bmBcA[] = getbmBc(P);
//        ArrayList<Integer> bmGs = getbmGs(P);
        int bmGsA[] = getbmGs(P);
//        int bmGsA[] = new int[bmGs.size()];
        char PA[] = P.toCharArray();
        char TA[] = T.toCharArray();
//        for(int i = 0;i<bmGs.size();i++){
//            bmGsA[i] = bmGs.get(i);
//        }

        int n = T.length(), m = P.length();
        int j = 0;
        while (j <= n - m) {
            int i = m - 1;
//            for (; i >= 0 && P.charAt(i) == T.charAt(i + j); --i) ;
            for (; i >= 0 && PA[i] == TA[i + j]; --i) ;
            if (i < 0) {
                result.add(j);
                i = 0;
            }
//            j += Math.max(bmGs.get(i), bmBc.get(T.charAt(i + j)) - m + i + 1);
            int v1=bmGsA[i], v2=bmBcA[TA[i + j]] - m + i + 1;
            j += v1>v2? v1:v2;
//            j += Math.max(bmGsA[i], bmBcA[TA[i + j]] - m + i + 1);
        }
        return result;
    }

//    public ArrayList<Integer> getbmGs(String P) {
    public int[] getbmGs(String P) {
//        ArrayList<Integer> Osuff = new ArrayList<>(P.length());
        int OsuffA[] = new int[P.length()];
//        ArrayList<Integer> bmGs = new ArrayList<>(P.length());
        int bmGsA[] = new int[P.length()];

        int m = P.length(), ov;

        // Compute Osuff
        for (int i = 0; i < m; ++i) {
//            Osuff.add(0);
            OsuffA[i] = 0;
        }
//        Osuff.set(m - 1, m);
        OsuffA[m - 1]=m;
        int misMatchPos = m - 1, MatchPos = m - 1;
        for (int i = m - 2; i >= 0; --i) {
            if (i > misMatchPos && (ov = OsuffA[i + m - 1 - MatchPos]) < i - misMatchPos) {
//            if (i > misMatchPos && (ov = Osuff.get(i + m - 1 - MatchPos)) < i - misMatchPos) {
                OsuffA[i]=ov;
//                Osuff.set(i, ov);
            } else {
                if (i < misMatchPos) {
                    misMatchPos = i;
                }
                MatchPos = i;
                while (misMatchPos >= 0 && P.charAt(misMatchPos) == P.charAt(misMatchPos + m - 1 - MatchPos))
                    --misMatchPos;
//                Osuff.set(i, MatchPos - misMatchPos);
                OsuffA[i] = MatchPos - misMatchPos;
            }
        }

        // Compute bmGs
        for (int i = 0; i < m; ++i) {
//            bmGs.add(m);
            bmGsA[i] = m;
        }
        int j = 0;
        for (int i = m - 2; i >= 0; --i) {
            if (OsuffA[i] == i + 1) {
//            if (Osuff.get(i) == i + 1) {
                while (j < m - 1 - i) {
//                    if (bmGs.get(j) == m)
                    if (bmGsA[j] == m)
//                        bmGs.set(j, m - 1 - i);
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
//                hm.put(P.charAt(i), sizeP - i - 1);
            hm[(int) P.charAt(i)] = sizeP - i - 1;
        }
        return hm;
    }
//    public static class BMBC {
////        HashMap<Character, Integer> hm;
//        int sizeP = 0;
//        public
//        int hm[] = new int [256];
//
//        public BMBC(String P) {
////            hm = new HashMap<>();
////            sizeP = P.length();
////            for (int i = 0; i < sizeP - 1; ++i) {
////                hm.put(P.charAt(i), sizeP - i - 1);
////            }
//            sizeP = P.length();
//            for (int i = 0; i < 256; ++i) {
//                hm[i] = sizeP;
//            }
//            for (int i = 0; i < sizeP - 1; ++i) {
////                hm.put(P.charAt(i), sizeP - i - 1);
//                hm[(int) P.charAt(i)] = sizeP - i - 1;
//            }
//        }
//
//        public Integer get(Character c) {
//            return hm[(int) c];
////            if (hm.containsKey(c)) return hm.get(c);
////            else return sizeP;
//        }
//    }
}

