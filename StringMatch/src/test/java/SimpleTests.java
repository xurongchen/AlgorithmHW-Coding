import org.junit.jupiter.api.Test;

import java.util.LinkedList;
import java.util.ListIterator;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class SimpleTests {

    @Test
    public void test1() {
        String T = "I'm a student in school of software.";
        String P = "school";
        Matcher matcher = new BruteForceMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 1);
        assertEquals(Result.getFirst(), 17);
    }

    @Test
    public void test2() {
        String T = "ABAABAABAAABBBBA";
        String P = "AAB";
        Matcher matcher = new BruteForceMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 3);
        ListIterator li = Result.listIterator();
        assertEquals(li.next(), 2);
        assertEquals(li.next(), 5);
        assertEquals(li.next(), 9);
    }

    @Test
    public void test3() {
        String T = "I'm a student in school of software.";
        String P = "school";
        Matcher matcher = new KmpMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 1);
        assertEquals(Result.getFirst(), 17);
    }

    @Test
    public void test4() {
        String T = "ABAABAABAAABBBBA";
        String P = "AAB";
        Matcher matcher = new KmpMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 3);
        ListIterator li = Result.listIterator();
        assertEquals(li.next(), 2);
        assertEquals(li.next(), 5);
        assertEquals(li.next(), 9);
    }

    @Test
    public void test5() {
        String T = "I'm a student in school of software.";
        String P = "school";
        Matcher matcher = new BmMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 1);
        assertEquals(Result.getFirst(), 17);
    }

    @Test
    public void test6() {
        String T = "ABAABAABAAABBBBA";
        String P = "AAB";
        Matcher matcher = new BmMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 3);
        ListIterator li = Result.listIterator();
        assertEquals(li.next(), 2);
        assertEquals(li.next(), 5);
        assertEquals(li.next(), 9);
    }

    @Test
    public void test7() {
        String T = "ANPANPANANMANMAN";
        String P = "ANPANMAN";
        Matcher matcher = new BmMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 0);
    }

    @Test
    public void test8() {
        String T = "GCAGAGAGCAGAGAGGCAGAGAGCAGAGAG";
        String P = "GCAGAGAG";
        Matcher matcher = new BmMatcher();
        LinkedList<Integer> Result = matcher.Match(T, P);
        assertEquals(Result.size(), 4);
        ListIterator li = Result.listIterator();
        assertEquals(li.next(), 0);
        assertEquals(li.next(), 7);
        assertEquals(li.next(), 15);
        assertEquals(li.next(), 22);
    }
}