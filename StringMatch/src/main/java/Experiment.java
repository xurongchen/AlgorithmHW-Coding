import java.nio.charset.Charset;
import java.security.SecureRandom;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Random;

public class Experiment {
    private static final String ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";
    private static final SecureRandom RANDOM = new SecureRandom();
    public static String randomString(int len){
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < len; ++i) {
            sb.append(ALPHABET.charAt(RANDOM.nextInt(ALPHABET.length())));
        }
        return sb.toString();
    }
    public HashMap<String, Object> run(Matcher matcher, String T, String P){
        long startTime = System.nanoTime();
        LinkedList<Integer> result = matcher.Match(T,P);
        long endTime = System.nanoTime();
        HashMap<String, Object> r = new HashMap<String, Object>();
        r.put("Time",endTime-startTime);
        r.put("Result",result);
        return r;
    }
}
