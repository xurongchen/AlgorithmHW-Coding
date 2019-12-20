import org.junit.jupiter.api.Test;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.ListIterator;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class DiffTests {

    @Test
    public void test1() {
        String T = Experiment.randomString(10000000);
        String P = Experiment.randomString(100);
        Matcher matcher = new BruteForceMatcher();
        HashMap<String, Object> ResultBM = new Experiment().run(new BmMatcher(),T,P);
        HashMap<String, Object> ResultBruteForce = new Experiment().run(new BruteForceMatcher(),T,P);
        HashMap<String, Object> ResultKMP = new Experiment().run(new KmpMatcher(),T,P);
        System.out.println(String.format("BF: %d; KMP: %d;BM: %d", ResultBruteForce.get("Time"),ResultKMP.get("Time"),ResultBM.get("Time")));

        LinkedList<Integer> RBF = (LinkedList<Integer>) ResultBruteForce.get("Result");
        LinkedList<Integer> RKM = (LinkedList<Integer>) ResultKMP.get("Result");
        LinkedList<Integer> RBM = (LinkedList<Integer>) ResultBM.get("Result");
        assertEquals(RBF.size(), RKM.size());
        assertEquals(RBF.size(), RBM.size());
        for(ListIterator it = RBF.listIterator(),jt = RKM.listIterator() ; it.hasNext();){
            assertEquals(it.next(), jt.next());
        }
        for(ListIterator it = RBF.listIterator(),jt = RBM.listIterator() ; it.hasNext();){
            assertEquals(it.next(), jt.next());
        }
    }

}