import java.util.LinkedList;

public interface Matcher {
    String Name();

    LinkedList<Integer> Match(String T, String P);
}
