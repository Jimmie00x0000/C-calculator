using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Text;
using System.Threading.Tasks;
using TemplateDefinition;
// 求幂 开方 定义常数
// 因为括号问题  需要  运算符压栈
//解析树包含完整表达式信息   用中缀表达式和前缀（LISP？）会损失信息，所以需要用括号补充
//用计算机的语言来表示   应该还原出解析树  
namespace NCalculator
{
    enum OperatorType//排序暗含优先级
    {
        ADD, SUB, MUL, DIV, POWER, SQRT,FACT,SIN,COS //想法：复杂运算分解成加减乘除在语法树中展开？
    }
  
    class OperateObj
    {
        private int ObjType;
        public virtual int getPriority()
        {
            return 0;
        }
        public int objType//属性 
        {
            get { return ObjType; }
            set { ObjType = value; }
        }
        public OperateObj(int t=0)
        {
            ObjType = t;
        }
    }
    
    class Operator:OperateObj{//
        public OperatorType type;
        public override int  getPriority()
        {
            switch (type)
            {
                case OperatorType.ADD:
                    return 1;
                    break;
                case OperatorType.SUB:
                    return 1;
                    break;
                case OperatorType.MUL:
                    return 2;
                    break;
                case OperatorType.DIV:
                    return 2;
                    break;
                case OperatorType.POWER:
                    return 3;
                    break;
                case OperatorType.SQRT:
                    return 3;
                    break;
                case OperatorType.FACT:
                    return 4;
                    break;
                case OperatorType.SIN:
                    return 4;//或者5？
                case OperatorType.COS:
                    return 4;
                default:
                    return 9;//For example,left bracket has lower priority than other operators
                    break;
            }
        }
        public Operator(OperatorType t): base(1)         
        {
            type = t;
        }
        
    }
    
    class Operand : OperateObj
    {
        public double OperandValue;
        public Operand(double v)
            : base(2)
        {
            OperandValue = v;
        }

    }
    
    class Bracket : OperateObj
    {
        public bool isLeft;
        public Bracket(bool L)
            : base(3)
        {
            isLeft = L;
        }
        public override int getPriority()
        {
            return 0;
        }
    }
    
    class calculator
    {
        const double PI = 3.1415926;
        const double E = 2.718281828;
        private int currErrorType;//0 no error  1 div 0  2 bracket can't match 3 1.1.1 4 - +
        private Stack<OperateObj> parseStack;
        private Stack<OperateObj> parseStack2;
        private BinaryTree<OperateObj> parseTree;
        private List<OperateObj> l;
        public calculator()
        {
            currErrorType = 0;
            parseStack = new Stack<OperateObj>();
            parseStack2 = new Stack<OperateObj>();
            //parseTree = new BinaryTree<OperateObj>();
            parseTree = null;
            l = new List<OperateObj>();
        }
        //private 
        //public static void ADD()
        //{  问题是 int float  没有模板编程  还有精度问题 还有数字长度问题

        //}
        public int CurrentErrorType
        {
            get { return currErrorType; }//forbidden set 
        }
        
        public double getResult(string str)
        {

            strDetect(str);
            if (buildParseTree())
                return finalComputeHelp(parseTree.Parent).OperandValue;
            return 0.0;
        }

        //构造解析树 迭代！    递归自下至上递归？  没见过  大概不行
        //迭代要遍历+操作符栈+操作数栈   递归只管当下
        //建树最好从顶向下建    
        private bool buildParseTree()  //先将RPN整到一个解析栈中  再通过栈递归建树
        {
            if (l.Count == 0)
                return false;
            if (currErrorType != 0)
                return false;
            parseStack.Clear();
            parseStack2.Clear();
        //    parseTree.clear();
           
            int index = 0;
           
            while (index != l.Count)//如果表达式格式非法 (应该在界面层防止这个问题发生）  建栈会如何？
            {
                OperateObj obj = l.ElementAt<OperateObj>(index);
                switch (obj.objType)
                {
                    case 1://操作符
                        Operator optTemp = (Operator)obj;
                        //Operator optStackTop;
                        //if (parseStack.Count==0)
                        //    optStackTop=null;
                        //else
                        //    optStackTop=(Operator)parseStack.Peek();

                        if (parseStack.Count == 0)
                        {
                            parseStack.Push(obj);
                        }
                        else
                        {
                            while (parseStack.Count!=0&&optTemp.getPriority() <= parseStack.Peek().getPriority())
                                parseStack2.Push(parseStack.Pop());
                            parseStack.Push(obj);
                        }
                             
                        break;
                    case 2://操作数 
                        parseStack2.Push(obj);
                        break;
                    case 3://括号
                        Bracket objTemp = (Bracket)obj;
                        if (objTemp.isLeft)//左括号压栈
                            parseStack.Push(obj);
                        else//右括号一直弹栈直到左括号出栈
                        {
                            while (parseStack.Peek().objType != 3)//不停弹栈
                            {
                                parseStack2.Push(parseStack.Pop());
                               // Operator objTemp2 = (Operator)parseStack.Pop();
                                //leftNode=new Node<OperateObj>()

                            }
                            parseStack.Pop();
                        }

                        break;
                    default:
                        break;
                }
                index++;
                
                //return new BinaryTree<OperateObj>(ln);   //这一行似乎没有必要         
                //parse 
            }
            while (parseStack.Count != 0)
                parseStack2.Push(parseStack.Pop());
            parseTree=new BinaryTree<OperateObj> (buildParseTreeHelp());
            if (CurrentErrorType == 4)
                return false;
            return true;
        }

        private Node<OperateObj> buildParseTreeHelp()//保证调用前栈顶一定是运算符
        {
            if (parseStack2.Count == 0)
                return null;
            if (parseStack2.Peek().objType != 1)
                return new Node<OperateObj>(parseStack2.Pop());
            OperateObj parentValue = parseStack2.Pop();
            Node<OperateObj> parentNode = null;
            Node<OperateObj> leftNode = null;
            Node<OperateObj> rightNode = null;//其实是指向一个parent的指针
            if (parseStack2.Count==0)
            {
                currErrorType = 4;
                return null;
            }
            switch (parseStack2.Peek().objType)//为右孩子操心
            {
                case 1://操作符
                    rightNode = buildParseTreeHelp();
                    break;
                case 2://操作数
                    rightNode = new Node<OperateObj>(parseStack2.Pop());//左子的左右子为null
                    break;
                default:
                    break;
                    
            }
            if (parseStack2.Count==0)
            {
                currErrorType = 4;
                return null;
            }
            switch (parseStack2.Peek().objType)//为左孩子操心
            {
                case 1://操作符
                    leftNode = buildParseTreeHelp();
                    break;
                case 2://操作数
                    leftNode = new Node<OperateObj>(parseStack2.Pop());
                    break;
                default:
                    break;
            }
            parentNode = new Node<OperateObj>(parentValue, leftNode, rightNode);
            return parentNode;
        }
        
        private Operand finalComputeHelp(Node <OperateObj> n)
        {
            if (parseTree == null || n == null)
            {
                currErrorType = 5;
                return new Operand (0.0);
            }
            if (parseStack2.Count>0)
            {
                currErrorType = 6;
                return new Operand(0);
            }
            if (n.NodeValue.objType == 2)//正常情况传入的n是操作符，但是这里是
            {//操作数   说明有异常
                return (Operand)n.NodeValue;
            }
            Operator obj=(Operator) n.NodeValue;
            Operand leftOperand = null;
            Operand rightOperand = null;
            switch (n.LeftChild.NodeValue.objType)
            {
                case 1:
                    leftOperand = finalComputeHelp(n.LeftChild);
                    break;
                case 2:
                    leftOperand = (Operand)n.LeftChild.NodeValue;
                    break;
            }
            switch (n.RightChild.NodeValue.objType)
            {
                case 1:
                    rightOperand = finalComputeHelp(n.RightChild);
                    break;
                case 2:
                    rightOperand = (Operand)n.RightChild.NodeValue;
                    break;
            }
            switch (obj.type)
            {
                case OperatorType.ADD:
                    return ADD(leftOperand, rightOperand);
                    break;
                case OperatorType.SUB:
                    return SUB(leftOperand, rightOperand);
                    break;
                case OperatorType.MUL:
                    return MUL(leftOperand, rightOperand);
                    break;
                case OperatorType.DIV:
                    return DIV(leftOperand, rightOperand);
                    break;
                case OperatorType.POWER:
                    return POWER(leftOperand, rightOperand);
                    break;
                case OperatorType.SQRT:
                    return SQRT(leftOperand, rightOperand);
                    break;
                case OperatorType.FACT:
                    return FACT(leftOperand, rightOperand);
                    break;
                case OperatorType.SIN:
                    return SIN(leftOperand, rightOperand);
                case OperatorType.COS:
                    return COS(leftOperand, rightOperand);

                default:
                    return null;
                    break;
            }
        }
        public void strDetect(string str)
        {
            l.Clear();
            char[] temp=new char [100];
            currErrorType = 0;//错误类型清零
            int indexTemp=0;
            int dotCount = 0;
            int leftBracketCount = 0;
            bool isDigit = false;
            //charArrayReset(temp);
            foreach (char c in str)// +-*/^$()   如果有非法字符？
            {
                if (c <= 57 && c >= 48 || c == '.')
                {
                    isDigit = true;
                    dotCount = c == '.' ? dotCount + 1 : dotCount;
                    if (dotCount > 1)
                    {
                        currErrorType = 3;
                        return;
                    }
                    temp[indexTemp++] = c;
                }
                else switch (c)
                {
                    case '+':
                        if (isDigit) { 
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }                      
                        l.Add(new Operator(OperatorType.ADD));//关于枚举类型
                        break;
                    case '-':
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }        
                        l.Add(new Operator(OperatorType.SUB));//关于枚举类型
                        break;
                    case '*':
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }        
                        l.Add(new Operator(OperatorType.MUL));//关于枚举类型
                        break;
                    case '/':
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }          
                        l.Add(new Operator(OperatorType.DIV));//关于枚举类型
                        break;
                    case '^':
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }          
                        l.Add(new Operator(OperatorType.POWER));//关于枚举类型
                        break;
                    case '!'://开方   补充一个左操作数，default=2 
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
          
                        }
                        
                        l.Add(new Operator(OperatorType.FACT));//关于枚举类型
                        l.Add(new Operand(1));//补充一个右操作数，default=1 
                        break;
                    case '$'://开方   补充一个左操作数，default=2 
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            l.Add(new Operator(OperatorType.MUL));//已经有数字，默认乘开方 
                        }
                        l.Add(new Operand(2));//补充一个左操作数，default=2 
                        l.Add(new Operator(OperatorType.SQRT));//关于枚举类型
                        break;
                    case '#'://开方   补充一个左操作数，default=2 
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            //l.Add(new Operator(OperatorType.MUL));//已经有数字，默认乘开方 
                        }
                      //  l.Add(new Operand(2));//补充一个左操作数，default=2 
                        l.Add(new Operator(OperatorType.SQRT));//关于枚举类型
                        break;
                    case 'P'://圆周率  乘号智能补全
                        if (isDigit)
                        {                          
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            l.Add(new Operator(OperatorType.MUL));
                            
                           // currErrorType = 6;
                        }
                        l.Add(new Operand(PI));
                        break;
                    case 'E'://圆周率  乘号智能补全
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            l.Add(new Operator(OperatorType.MUL));

                            // currErrorType = 6;
                        }
                        l.Add(new Operand(E));
                        break;
                    case 'S'://圆周率  乘号智能补全
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            l.Add(new Operator(OperatorType.MUL));

                            // currErrorType = 6;
                        }
                        l.Add(new Operand(1));
                        l.Add(new Operator(OperatorType.SIN));
                        break;
                    case 'C'://圆周率  乘号智能补全
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                            l.Add(new Operator(OperatorType.MUL));

                            // currErrorType = 6;
                        }
                        l.Add(new Operand(1));
                        l.Add(new Operator(OperatorType.COS));
                        break;
                    case '('://注意括号不属于运算符  不用将数字压栈 
                        leftBracketCount++;
                        l.Add(new Bracket(true));//关于枚举类型
                        break;
                    case ')':
                        leftBracketCount--;
                        Console.WriteLine("leftBracketCount: {0}", leftBracketCount);
                        if (leftBracketCount<0)//防止括号不匹配
                        {
                            currErrorType = 2;
                            return;
                        }
                        if (isDigit)
                        {
                            l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                            isDigit = false;
                        }   
                        l.Add(new Bracket(false));//关于枚举类型
                        break;
                    default:
                        
                        break;
                }
                    // double d=temp.t
            }
            if (isDigit)
            {
                l.Add(new Operand(charArrayToDouble(temp, ref indexTemp)));
                isDigit = false;
            }  
            while (leftBracketCount!=0)
            {
                leftBracketCount--;
                l.Add(new Bracket(false));//右括号补全

            }

           // Console.WriteLine("break");

        }
        private void charArrayReset(char[] temp)
        {
            for (int i = 0; i < temp.Length; i++)
                temp[i] = 'N';
        }
        public double charArrayToDouble(char[] temp,ref int currLength)
        {
            double tempValue=0.0;//整数值
            double fractionalPart = 0.0;//小数部分
            int w;//位数
            for ( w = 0; w<currLength&&temp[w]!='.'; w++)
            {
                
                tempValue *= 10;
                tempValue += temp[w]-48;

            }
           // Console.WriteLine("{0}", tempValue);
            for (int ew=currLength-1; ew > w;ew--)
            {
                
                fractionalPart += temp[ew]-48;
                fractionalPart /= 10;
            }
            tempValue += fractionalPart;
            currLength = 0;
            return tempValue;
        }
        public static double SQRT(int t1, int t2)
        {
            return 1.0;
        }
        public static Operand SQRT(Operand t1, Operand t2)//t1开方次数 t2被开方数
        {
            int c = (int)t1.OperandValue;
            double n = t2.OperandValue;
            double k = n;//初始猜测值取小了就不断增 取大了就不断减少
            //while (ABS(POWER(k,c)- n) > 1e-9)
            //{
            //    k = (k + n / k) / 2;
            //}
            while (ABS(POWER(k, c) - n) > 1e-9)//牛顿迭代法
            {
                k -= (POWER(k, c)-n) / (c * POWER(k, c - 1));
            }//利用迭代公式 x(n+1)=x(n)－f(x(n))/f'(x(n))
             
            return new Operand(k);
        }
        public static Operand POWER(Operand t1, Operand t2)
        {
            double l =  t1.OperandValue;
            int r = (int)t2.OperandValue;
            return new Operand((double)POWER(l, r));
        }
        public static int POWER(int t1, int t2)
        {
            if (t2 > 0) return t1 *= POWER(t1,--t2);
            else return 1;
        }
        public static double POWER(double t1, int t2)
        {
            if (t2 > 0) return t1 *= POWER(t1, --t2);
            else return 1.0;
        }
        public static Operand ADD(Operand t1, Operand t2)//int 4 bytes 
        {
            return new Operand(t1.OperandValue+t2.OperandValue);//考虑越界问题
        }
        public static double ADD(double t1, double t2)
        {
            return t1 + t2;//考虑越界问题
        }
        public static Operand SUB(Operand t1, Operand t2)//int 4 bytes 
        {
            return new Operand(t1.OperandValue - t2.OperandValue);//考虑越界问题
        }
        public static double SUB(double t1, double t2)
        {
            return t1 - t2;
        }
        public static Operand MUL(Operand t1, Operand t2)//int 4 bytes 
        {
            return new Operand(t1.OperandValue * t2.OperandValue);//考虑越界问题
        }
        public static double MUL(double t1, double t2)
        {
            return t1 * t2;
        }
        public  Operand DIV(Operand t1, Operand t2)//int 4 bytes 
        {
            //if (System.Math.Abs(t2.OperandValue - 0) < 1e-8)
            if (ABS(t2.OperandValue - 0) < 1e-8)
            {
                currErrorType = 1;
                return new Operand(0.0);
            }
            return new Operand(t1.OperandValue / t2.OperandValue);//考虑越界问题
        }
        public static double DIV(double t1, double t2)
        {
            return t1 / t2;
        }
        public static double ABS(double s)
        {
            return s >= 0 ? s : -s;
        }
        public Operand FACT(Operand t1, Operand t2)
        {
            int l = (int)t1.OperandValue;
            return new Operand(FACT(l,1));
        }
        public static int FACT(int t1, int t2)
        {
            if (t1 == 0)
                return 1;
            return t1 * FACT(t1 - 1,1);
        }
        public static Operand SIN(Operand t1, Operand t2)  //控制在 0-2PI
        {
            double v = t2.OperandValue < 0 ? t2.OperandValue % (2 * PI) +2*PI: t2.OperandValue % (2 * PI);
            return new Operand(SIN(v));
        }
        public static Operand COS(Operand t1, Operand t2)
        {
            double v = t2.OperandValue < 0 ? t2.OperandValue % (2 * PI) + 2 * PI : t2.OperandValue % (2 * PI);
            return new Operand(COS(v));
        }
        public static double SIN(double t)//泰勒级数展开
        {
            double sinV=0;
            int k = 0 ;
            while (k++ <  11)//这个9不能取大  否则计算过程中会溢出
            {
                double delta = k % 2 == 0 ? -POWER(t, 2 * k - 1) / FACT(2 * k - 1, 1) : POWER(t, 2 * k - 1) / FACT(2 * k - 1, 1);
                sinV+=delta;
            }
            return sinV;
        }
        public static double COS(double t)
        {
            double cosV = 1.0;
            int k = 0;
            while (k++ < 13)
            {
                double delta = k % 2 == 0 ? POWER(t, 2 * k ) / FACT(2 * k , 1) : -POWER(t, 2 * k ) / FACT(2 * k , 1);
                cosV += delta;
            }
            return cosV;
        }
    }
}
