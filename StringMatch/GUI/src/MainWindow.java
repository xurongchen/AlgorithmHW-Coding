import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.LinkedList;

public class MainWindow {
    private JButton openFileButton;
    private JPanel mainPanel;
    private JTextPane ResultText;
    private JButton runButton;
    private JComboBox AlgorithmChoose;
    private JTextField textP;
    private JLabel filePathLabel;
    private JLabel TimeText;
    private String TStringFile = "";


    public MainWindow() {
        openFileButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFileChooser jfc=new JFileChooser();
                jfc.setFileSelectionMode(JFileChooser.FILES_ONLY );
                jfc.showDialog(new JLabel(), "Choose");
                File file=jfc.getSelectedFile();
                if(file != null){
                    filePathLabel.setText("File Path: "+file.getAbsolutePath());
                    System.out.println(file.getAbsolutePath());
                    TStringFile = file.getAbsolutePath();
                }
                else System.out.println("No choose");
            }
        });
        runButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                Matcher matcher;
                switch (AlgorithmChoose.getSelectedIndex()){
                    case 0:
                        matcher = new BruteForceMatcher();
                        break;
                    case 1:
                        matcher = new KmpMatcher();
                        break;
                    default:
                        matcher = new BmMatcher();
                }
                try {
                    String T = Experiment.readString(TStringFile);
                    String P = textP.getText();
                    if(P.length()<1){
                        JOptionPane.showMessageDialog(null, "Pattern length is 0!", "Error",JOptionPane.ERROR_MESSAGE);
                        return;
                    }
                    HashMap<String, Object> Result = new Experiment().run(matcher,T,P);
                    long Runtime = (long) Result.get("Time");
                    LinkedList<Integer> Matches = (LinkedList<Integer>) Result.get("Result");
                    TimeText.setText(String.format("Time: %dms", Runtime/1000000));
                    StringBuilder sb = new StringBuilder();
                    sb.append("Algorithm name is: " + matcher.Name() + "\n");
                    sb.append(String.format("Text size is %d and pattern size is %d.\n", T.length(), P.length()));
                    sb.append(String.format("Totally, there are %d positions matched. They are: \n", Matches.size()));
                    for(int i = 0; i < Matches.size(); ++i){
                        sb.append(String.format("[%d]: Position @ %d;\n", i+1, Matches.get(i)));
                    }
                    ResultText.setText(sb.toString());
                } catch (IOException ex) {
                    ex.printStackTrace();
                    JOptionPane.showMessageDialog(null, "Error in file read!", "Error",JOptionPane.ERROR_MESSAGE);
                }

            }
        });
    }

    public static void main(String[] args){
        JFrame frame = new JFrame("String Matcher");
        frame.setContentPane(new MainWindow().mainPanel);

        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.pack();
        frame.setVisible(true);
        frame.setSize(500,300);
    }
}
